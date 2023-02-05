using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class CameraCollider : MonoBehaviour
    {
        public event Action OnEnterCollider;
        public event Action OnExitCollider;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Root"))
            {
                OnEnterCollider?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Root"))
            {
               // OnExitCollider?.Invoke();
            }
        }



    }
}