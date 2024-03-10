using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Entities
{
    internal class CheckFly : MonoBehaviour
    {
        public bool IsFly { get; private set; }
        [SerializeField] private float _radius = 0.2f;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _coyoteTime = 0.15f;

        private float flyTimer = 0;

        private void Update()
        {
            if (Physics.CheckSphere(transform.position, _radius, _layerMask))
            {
                IsFly = false;
                flyTimer = 0;
            }
            else
            {
                flyTimer += Time.deltaTime;
                if (flyTimer > _coyoteTime)
                {
                    IsFly = true;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        } 
#endif

    }
}
