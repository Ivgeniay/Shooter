using CodeBase.Entities;
using UnityEngine;

namespace CodeBase.Animations
{
    internal class CharacterAnimationController : MonoBehaviour
    {
        private readonly int GroundedPrm = Animator.StringToHash("Grounded");
        private readonly int SpeedPrm = Animator.StringToHash("Speed");

        [SerializeField] private Animator _animator;
        [SerializeField] private Character _character;
        [SerializeField] private CheckFly _checkFly;

        private void Update()
        {
            Vector3 localVelocity = _character.transform.InverseTransformVector(_character.Velocity);
            float speed = localVelocity.magnitude / _character.Speed;
            float sign = Mathf.Sign(localVelocity.z);

            _animator.SetFloat(SpeedPrm, speed * sign);
            _animator.SetBool(GroundedPrm, !_checkFly.IsFly);
        }
    }
}
