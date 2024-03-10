using UnityEngine; 
using Colyseus;
using System;
using System.Collections.Generic;
using CodeBase.Entities;

namespace CodeBase.Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private PlayerCharacter _playerPrefab;
        [SerializeField] private EnemyController _enemyPrefab;
        private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();

        private ColyseusRoom<State> _room;

        protected override void Awake()
        {
            base.Awake();
            Instance.InitializeClient();
            Connect();
        }

        private async void Connect()
        {
            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "speed", _playerPrefab.Speed }
            };

            _room = await Instance.client.JoinOrCreate<State>("state_handler", options);
            _room.OnStateChange += OnStateChangeHandler;
            _room.OnMessage<string>("Shoot", OnApplyShootHandler);
        }

        private void OnStateChangeHandler(State state, bool isFirstState)
        {
            _room.OnStateChange -= OnStateChangeHandler;

            state.players.ForEach((key, player) =>
            {
                if (key == _room.SessionId) CreatePlayer(player); 
                else CreateEnemy(key, player);  
            });

            _room.State.players.OnAdd += OnPlayerAdd;
            _room.State.players.OnRemove += OnRemovePlayer;
        } 
        private void OnPlayerAdd(string key, Player value)
        {
            CreateEnemy(key, value);
        } 
        private void OnRemovePlayer(string key, Player value)
        {
            if (_enemies.ContainsKey(key) == false)
                return;

            EnemyController enemy = _enemies[key];
            enemy.Destroy();

            _enemies.Remove(key);
        }

        private void CreatePlayer(Player player) =>
            Instantiate(_playerPrefab, new Vector3(player.pX, player.pY, player.pZ), Quaternion.identity); 

        private void CreateEnemy(string key, Player player)
        {
            EnemyController enemy = Instantiate(_enemyPrefab, new Vector3(player.pX, player.pY, player.pZ), Quaternion.identity);
            enemy.Init(player);

            _enemies.Add(key, enemy);
        }

        private void OnApplyShootHandler(string jsonShootInfo)
        {
            ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);

            if (_enemies.ContainsKey(shootInfo.key) == false)
            {
                Debug.LogError($"Id {shootInfo.key} does not exist.");
                return;
            }

            _enemies[shootInfo.key].Shoot(shootInfo);
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

        internal void SendMessage(string header, string data)
        {
            _room.Send(header, data);
        }

        internal string GetSessionID()
        {
            return _room.SessionId;
        }
    }
}
