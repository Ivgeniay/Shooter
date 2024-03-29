﻿using CodeBase.Entities.Guns;
using UnityEngine;

namespace CodeBase.Entities.Guns
{
    internal class PlayerGun : Gun
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _shootDelay = 0.2f;

        private float _lastShootTime;

        public bool TryShoot(out ShootInfo info)
        {
            info = new ShootInfo();

            if (Time.time - _lastShootTime < _shootDelay)
                return false;

            Vector3 position = _shootPoint.position;
            Vector3 velocity = _shootPoint.forward * _bulletSpeed;
            _lastShootTime = Time.time;

            Instantiate(BulletPrefab, position, _shootPoint.rotation)
                .Init(velocity);

            info.pX = position.x;
            info.pY = position.y;
            info.pZ = position.z;

            info.dX = velocity.x;
            info.dY = velocity.y;
            info.dZ = velocity.z;

            OnShootEvent?.Invoke();
            return true;
        }
    }
}
