using UnityEngine;

namespace CodeBase.Entities
{
    internal class EnemyCharacter : MonoBehaviour
    {
        public void SetPosition(Vector3 position) => transform.position = position;
    }
}
