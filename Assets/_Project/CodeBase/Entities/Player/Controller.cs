using CodeBase.Multiplayer;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter _player;

        void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical"); 
            _player.SetInput(h, v);

            if (v != default || h != default)
            {
                SendMove();
            }
        }

        private void SendMove()
        {
            _player.GetMoveInfo(out var position);
            Dictionary<string, object> move = new Dictionary<string, object>
            {
                { "x", position.x },
                { "y", position.z }
            };
            MultiplayerManager.Instance.SendMessage("move", move);
        }
    }
}
