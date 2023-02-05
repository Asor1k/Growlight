using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class CameraBehaviour : MonoBehaviour
    {
        public event Action<Vector3> OnMovedDelta;
        public float Speed => speed;

        [SerializeField] private CameraCollider cameraCollider;
        [SerializeField] private float speed;
        [SerializeField] private Vector3 direction;
        [SerializeField] private float speedIncreese;
        [SerializeField] private RootControl _rootControl;
        
        private int numberOfRoots;

        private void Start()
        {
            numberOfRoots = 0;
            cameraCollider.OnEnterCollider += SetMoving;
            cameraCollider.OnExitCollider += DecreaseMoving;
        }

        

        private void SetMoving()
        {
            numberOfRoots++;
        }

        private void DecreaseMoving()
        {
            numberOfRoots--;
        }

        private void Update()
        {

            if (_rootControl.CanGrow())
            {
                Vector3 delta = direction * speed * Time.deltaTime;
                transform.position += delta;
                OnMovedDelta?.Invoke(delta);
                speed += speedIncreese * Time.deltaTime;
            }
        }



    }
}