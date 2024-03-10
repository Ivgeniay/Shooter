using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine; 
using System.Linq; 
using CodeBase.Entities.Guns;

namespace CodeBase.Entities
{
    [RequireComponent(typeof(EnemyCharacter))]
    internal class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyGun _gun;
        private EnemyCharacter _character;
        private List<float> receiveTimeInterval = new List<float>() { 0, 0, 0, 0, 0 };
        private float _deltaAnswerTime { get => receiveTimeInterval.Average(); }
        private float _lastReceiveTime = 0;
        
        private Player _player;

        private void Awake()
        {
            _character = GetComponent<EnemyCharacter>();
        }

        public void Init(Player player)
        {
            this._player = player;
            _character.SetSpeed(player.speed);
            player.OnChange += OnChange; 
        }

        public void Shoot(in ShootInfo info)
        {
            Vector3 position = new Vector3(info.pX, info.pY, info.pZ);
            Vector3 velocity = new Vector3(info.dX, info.dY, info.dZ);

            _gun.Shoot(position, velocity);
        }

        public void Destroy()
        {
            Destroy(gameObject);
            _player.OnChange -= OnChange;
        }

        private void SaveRecieveTime()
        {
            float interaval = Time.time - _lastReceiveTime;
            _lastReceiveTime = Time.time;

            receiveTimeInterval.Add(_lastReceiveTime);
            receiveTimeInterval.RemoveAt(0);
        }


        internal void OnChange(List<DataChange> changes)
        {
            SaveRecieveTime();
            Vector3 position = _character.TargetPosition;
            Vector3 velocity = _character.Velocity;

            foreach(DataChange change in changes)
            {
                switch(change.Field)
                {
                    case "pX":
                        position.x = (float)change.Value;
                        break;
                    case "pY":
                        position.y = (float)change.Value;
                        break;
                    case "pZ":
                        position.z = (float)change.Value;
                        break;

                    case "vX":
                        velocity.x = (float)change.Value;
                        break;
                    case "vY":
                        velocity.y = (float)change.Value;
                        break;
                    case "vZ":
                        velocity.z = (float)change.Value;
                        break;

                    case "rX":
                        _character.SetRotateX((float)change.Value);
                        break;
                    case "rY":
                        _character.SetRotateY((float)change.Value);
                        break;

                    default:
                        Debug.LogWarning("Not implemented field");
                            break;
                }
            }

            _character.SetMovement(position, velocity, _deltaAnswerTime);
        }
    }
}
