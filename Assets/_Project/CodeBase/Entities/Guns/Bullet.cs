using System.Collections;
using UnityEngine;

namespace CodeBase.Entities.Guns
{
    internal class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 5f;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(Vector3 velocity)
        {
            _rigidbody.velocity = velocity;
            StartCoroutine(DelayDestroy());
        }

        private void OnCollisionEnter(Collision other) =>
            Destroy();

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(_lifeTime);
            Destroy();
        }

        private void Destroy() =>
            Destroy(gameObject);
    }
}
