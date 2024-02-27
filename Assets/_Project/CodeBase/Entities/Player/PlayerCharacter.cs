using UnityEngine;

namespace CodeBase
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        private float _inputH;
        private float _inputV;

        public void SetInput(float h, float v)
        {
            _inputH = h;
            _inputV = v;
        }

        private void Update()
        {
            Vector3 direction = new Vector3(_inputH, 0, _inputV);
            Move(direction.normalized);
        }

        public void GetMoveInfo(out Vector3 position)
        {
            position = transform.position;
        }

        private void Move(Vector3 direction)
        { 
            transform.position += direction * Time.deltaTime * _speed;
        }
    }
}
