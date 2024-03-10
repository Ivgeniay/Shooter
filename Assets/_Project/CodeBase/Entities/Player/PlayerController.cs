using CodeBase.Entities.Guns;
using CodeBase.Multiplayer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase
{
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerGun _gun;
        [SerializeField] private float _mouseSensitivity = 2f;

        private MultiplayerManager _multiplayerManager { get => MultiplayerManager.Instance; }
        private PlayerCharacter _player;

        private void Awake() =>
            _player = GetComponent<PlayerCharacter>();

        void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical"); 

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            bool isShoot = Input.GetMouseButton(0);

            bool space = Input.GetKeyDown(KeyCode.Space);

            _player.SetInput(h, v, mouseX * _mouseSensitivity);
            _player.RotateX(-mouseY * _mouseSensitivity);
            if (space) _player.Jump();
            if (isShoot && _gun.TryShoot(out ShootInfo shootInfo)) 
                SendShoot(ref shootInfo);

            SendMove();
        }

        private void SendMove()
        {
            _player.GetMoveInfo(out var position, out Vector3 velocity, out float rotateX, out float rotateY);
            Dictionary<string, object> move = new Dictionary<string, object>
            {
                { "pX", position.x },
                { "pY", position.y },
                { "pZ", position.z },
                { "vX", velocity.x },
                { "vY", velocity.y },
                { "vZ", velocity.z },
                { "rX", rotateX },
                { "rY", rotateY }
            };
            _multiplayerManager.SendMessage("move", move);
        }

        private void SendShoot(ref ShootInfo shootInfo)
        {
            shootInfo.key = _multiplayerManager.GetSessionID();
            string json = JsonUtility.ToJson(shootInfo);
            _multiplayerManager.SendMessage("shoot", json);
        }

    }

    [Serializable]
    public struct ShootInfo
    {
        public string key;
        public float pX;
        public float pY;
        public float pZ;
        public float dX;
        public float dY;
        public float dZ;
    }
}
