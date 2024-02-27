using UnityEngine; 
using Colyseus;
using System;
using System.Collections.Generic;
using CodeBase.Entities;

namespace CodeBase.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private EnemyController _enemyPrefab;

        private ColyseusRoom<State> _room;

        protected override void Awake()
        {
            base.Awake();
            Instance.InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            _room = await Instance.client.JoinOrCreate<State>("state_handler");
            _room.OnStateChange += OnStateChangeHandler;
        } 
        private void OnStateChangeHandler(State state, bool isFirstState)
        {
            if (!isFirstState) return;
             
            state.players.ForEach((key, player) =>
            {
                if (key == _room.SessionId) CreatePlayer(player); 
                else CreateEnemy(key, player);  
            });

            _room.State.players.OnAdd += OnPlayerAdd;
            _room.State.players.OnRemove += RemoveEnemy;
        } 
        private void OnPlayerAdd(string key, Player value)
        {
            CreateEnemy(key, value);
        } 
        private void RemoveEnemy(string key, Player value)
        {

        }

        private void CreatePlayer(Player player) =>
            Instantiate(_playerPrefab, new Vector3(player.x, 0, player.y), Quaternion.identity); 

        private void CreateEnemy(string key, Player player)
        {
            EnemyController enemy = Instantiate(_enemyPrefab, new Vector3(player.x, 0, player.y), Quaternion.identity);
            player.OnChange += enemy.OnChange;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _room.Leave();
        }

        internal void SendMessage(string header, Dictionary<string, object> data)
        {
            _room.Send(header, data);
        }
    }

    public class Room
    {
        public float Score;
        public float Time;
    }
}
