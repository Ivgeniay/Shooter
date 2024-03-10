﻿using UnityEngine;

namespace CodeBase.Entities
{
    internal class EnemyCharacter : Character
    {
        [SerializeField] private Transform _headModel;

        public Vector3 TargetPosition { get; private set; } = Vector3.zero;
        private float _velocityMagnitude;

        private void Start()
        {
            TargetPosition = transform.position;
        }

        private void Update()
        {
            if (_velocityMagnitude > 0.01f)
            { 
                float maxDistance = _velocityMagnitude * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, maxDistance);
            }
            else
            {
                transform.position = TargetPosition;
            }
        }

        public void SetSpeed(float value) => Speed = value; 
        public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
        {
            TargetPosition = position + velocity * averageInterval;
            _velocityMagnitude = velocity.magnitude;

            this.Velocity = velocity;
        } 
        public void SetRotateX(float value)
        {
            _headModel.localEulerAngles = new Vector3(value, 0, 0);
        }
        public void SetRotateY(float value)
        {
            transform.localEulerAngles = new Vector3(0, value, 0);
        }
    }
}
