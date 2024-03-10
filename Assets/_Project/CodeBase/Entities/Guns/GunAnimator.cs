using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Entities.Guns
{
    internal class GunAnimator : MonoBehaviour
    {
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        [SerializeField] private Gun _gun;
        [SerializeField] private Animator _animator;

        private void Start() =>
            _gun.OnShootEvent += OnShootHappened;

        private void OnDestroy() =>
            _gun.OnShootEvent -= OnShootHappened;

        private void OnShootHappened() =>
            _animator.SetTrigger(Shoot);
    }
}
