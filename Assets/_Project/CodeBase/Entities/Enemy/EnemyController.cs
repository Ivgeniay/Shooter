using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;
using System;

namespace CodeBase.Entities
{
    internal class EnemyController : MonoBehaviour
    {
        internal void OnChange(List<DataChange> changes)
        {
            foreach(DataChange change in changes)
            {
                switch(change.Field)
                {
                    case "x":
                        transform.position = new Vector3((float)change.Value, transform.position.y, transform.position.z);
                        break;
                    case "y":
                        transform.position = new Vector3(transform.position.x, transform.position.y, (float)change.Value);
                        break;

                    default:
                        Debug.LogWarning("Not implemented field");
                            break;
                }
            }
        }
    }
}
