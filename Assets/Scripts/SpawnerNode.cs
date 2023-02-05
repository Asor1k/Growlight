using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class SpawnerNode : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 direction;
        [SerializeField] private Vector3 shootingDirection;

        [SerializeField] private Root mainRoot;
        [SerializeField] private CameraBehaviour cameraBehaviour;
        [SerializeField] private RootControl control;

        public Vector3 GetShootingDirection()
        {
            return shootingDirection;
        }
        

        private void Update()
        {
            if (control.CanGrow())
            {
                transform.position += direction * cameraBehaviour.Speed * Time.deltaTime;
            }
        }
    }
}