using System;
using UnityEngine;

namespace CodeBase.Entities.Guns
{
    internal abstract class Gun : MonoBehaviour
    {
        [SerializeField] protected Bullet BulletPrefab;

        public Action OnShootEvent;
    }
}
