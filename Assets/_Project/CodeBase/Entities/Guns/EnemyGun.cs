using UnityEngine;

namespace CodeBase.Entities.Guns
{
    internal class EnemyGun : Gun
    {

        public void Shoot(Vector3 position, Vector3 velocity)
        {
            Instantiate(BulletPrefab, position, Quaternion.identity)
                .Init(velocity);

            OnShootEvent?.Invoke();
        }
    }
}
