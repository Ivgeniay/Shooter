using CodeBase.Entities;
using UnityEngine;

namespace CodeBase
{
    [RequireComponent(typeof(Rigidbody))]
    internal class PlayerCharacter : Character
    {
        [SerializeField] private CheckFly _checkFly;

        [SerializeField] private Transform _headModel;
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private float _jumpForce = 50f;
        [SerializeField] private float _jumpDelay = 0.2f;

        [SerializeField] private float _maxHeadAngle = 90f;
        [SerializeField] private float _minHeadAngle = -90f;
        
        private Rigidbody _rigidbody;
        private float _inputH;
        private float _inputV;
        private float _rotateY;
        private float _currentHeadAngle;

        private float _jumpTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            Transform camera = Camera.main.transform;
            camera.SetParent(_cameraPoint);
            camera.localPosition = Vector3.zero;
            camera.localRotation = Quaternion.identity;
        }


        private void FixedUpdate()
        {
            Move();
            RotateY();
        } 

        public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
        {
            position = transform.position;
            velocity = _rigidbody.velocity;
            rotateY = transform.eulerAngles.y;
            rotateX = _headModel.localEulerAngles.x;
        }
        public void RotateX(float value)
        {
            _currentHeadAngle = Mathf.Clamp(_currentHeadAngle + value, _minHeadAngle, _maxHeadAngle);
            _headModel.localEulerAngles = new Vector3(_currentHeadAngle, 0, 0);
        } 
        public void SetInput(float h, float v, float rotateY)
        {
            _inputH = h;
            _inputV = v;
            _rotateY += rotateY;
        }
        public void Jump()
        {
            if (_checkFly.IsFly) return;
            if (Time.time - _jumpTime < _jumpDelay) return;

            _jumpTime = Time.time;
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        }
        private void Move()
        {
            Vector3 velocity = (transform.forward * _inputV + transform.right * _inputH).normalized * Speed;
            velocity.y = _rigidbody.velocity.y;
            base.Velocity = velocity;
            _rigidbody.velocity = base.Velocity;
        } 
        private void RotateY()
        {
            _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
            _rotateY = 0;
        }
    }
}
