using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jam
{
    [RequireComponent(typeof(Rigidbody))]
    public class DarkBuddy : MonoBehaviour
    {
        public event Action OnDied;

        [SerializeField]
        private Rigidbody rb;

        [SerializeField] private float verticalSpeed;
        [SerializeField] private float 
            horizontalSpeed;

        [SerializeField] private float minForce;
        [SerializeField] private float maxForce;
        [SerializeField] private Animator animator;

        private Vector3 offset;
        private Root root;

        public void Init(Vector3 direction)
        {
            StartCoroutine(Moving(direction, Random.Range(minForce, maxForce)));
            //rb.AddForce(direction * );
        }

        private IEnumerator Moving(Vector3 direction, float speed)
        {
            while (true)
            {
                Vector3 moveDirection = new Vector3(direction.x, direction.y + Mathf.Sin(Time.time * verticalSpeed) * horizontalSpeed);
                transform.position += moveDirection * speed * Time.deltaTime; //Vector3.MoveTowards(transform.position], )
                yield return null;
            }
        }

        private void OnDisable()
        {
            OnDied?.Invoke();
        }


        public void SetRoot(Root root)
        {
            offset = transform.position - root.transform.position;
            this.root = root;
        }

        private void UnStick()
        {
            root.UnStick(this);
        }

        private void Stick(Root root)
        {
            StopAllCoroutines();
            Transform closestPoint = root.GetClosestPoint(transform.position);
            SetRoot(root);
            AudioManager.Instance.PlayEffect(Audio.BuddieTouch);
            root.Stick(this);
            animator.SetBool("IsSucking", true);
            rb.isKinematic = true;
        }

        public void Die()
        {
            if (this.root)
            {
                UnStick();
            }
            print("Die");
            if (this)
            {
                DestroyImmediate(gameObject);
            }
        }

        private void Update()
        {
            if (!root) return;
            transform.position = root.transform.position + offset;
        }


        private void OnTriggerEnter(Collider other)
        {
            Root root;
            if (other.TryGetComponent<Root>(out root))
            {
                Stick(root);
                return;
            }

            //WispLight wispLight;

            //if (other.TryGetComponent<WispLight>(out wispLight))
            //{
            //    if (this.root)
            //    {
            //        UnStick();
            //    }
            //    //wispLight.Pulse();
            //    //Die();
            //}
        }

    }
}