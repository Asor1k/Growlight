using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class WispLight : MonoBehaviour
    {
        [SerializeField] private float force;
        [SerializeField] private float timeToStop;

        [SerializeField] private float growSpeed;
        [SerializeField]
        private RootControl rootControl;

        [SerializeField] private float pulseCooldown;

        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer gfx;
        [SerializeField] private float animationSpeed;
        [SerializeField] private float radius;

        private float initialForce;
        private Wisp wisp;
        private float timeSinceLastPulse;
        private List<DarkBuddy> darkBuddiesAround = new List<DarkBuddy>();
        private bool isDragging;
        


        private void Start()
        {
            initialForce = force;
        }

        private void OnTriggerEnter(Collider other)
        {
            DarkBuddy darkBuddy;
            if (other.TryGetComponent(out darkBuddy))
            {
                darkBuddiesAround.Add(darkBuddy);
                darkBuddy.OnDied += ClearDeadBuddies;
                print(Time.time - timeSinceLastPulse);
                if (Time.time - timeSinceLastPulse > pulseCooldown)
                {
                    Pulse();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            DarkBuddy darkBuddy;
            if (other.TryGetComponent(out darkBuddy))
            {
                darkBuddiesAround.Remove(darkBuddy);
            }
        }

        private void ClearDeadBuddies()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(ClearInFrame());
            }
        }

        private IEnumerator ClearInFrame()
        {
            yield return new WaitForEndOfFrame();
            darkBuddiesAround.RemoveAll(buddie => buddie == null);
        }

        public void StartDragging(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wisp"))
            {
                wisp = other.GetComponent<Wisp>();
            }
        }

        public void Drag(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wisp"))
            {
                if (Vector3.Distance(transform.position, other.transform.position) < 2f)
                {
                    isDragging = false;
                }
                else
                {
                    isDragging = true;
                    gfx.transform.localPosition =
                        Vector3.Lerp(gfx.transform.localPosition, Vector3.zero, Time.deltaTime);
                }


                transform.position = Vector3.MoveTowards(transform.position, other.transform.position,
                        force * (3 / Vector3.Distance(transform.position, other.transform.position)) * Time.deltaTime);
                    Vector3 diff = other.transform.position - transform.position;
                    transform.position += new Vector3(Mathf.Sign(diff.x), Mathf.Sign(diff.y), 0) * force / 20 *
                                          Time.deltaTime * Mathf.Sin(Time.time);
                
            }
        }

        public void EndDragging()
        {
            isDragging = false;
        }

        private void Pulse()
        {
            if (Time.time - timeSinceLastPulse < pulseCooldown) return;
            timeSinceLastPulse = Time.time;
            AudioManager.Instance.PlayEffect(Audio.PulseWave);
            animator.SetTrigger("TR_Pulse");
            StartCoroutine(Pulsing());
        }

        private IEnumerator Pulsing()
        {
            yield return new WaitForSeconds(0.3f);
            darkBuddiesAround.ForEach(buddy => buddy.Die());
            rootControl.RemoveSticked(null);
            StartCoroutine(WaitingToCheckBuddies());
        }

        private IEnumerator WaitingToCheckBuddies()
        {
            yield return new WaitForSeconds(pulseCooldown - 0.3f);
            if (darkBuddiesAround.Count != 0)
            {
                Pulse();
            }
        }
        

        private void Update()
        {
            if (!isDragging)
            {
                Vector3 circleVector = new Vector3(Mathf.Cos(Time.time * animationSpeed),
                    Mathf.Sin(Time.time * animationSpeed) * Mathf.Cos(Time.time * animationSpeed));

                gfx.transform.localPosition = Vector3.Lerp(gfx.transform.localPosition, circleVector * radius, Time.deltaTime * 2);
            }

            if (rootControl.CanGrow())
            {
                transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
                
                force = initialForce * transform.localScale.x * 2;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
        }

        //private IEnumerator Stopping()
        //{
        //    float start = Time.time;

        //    while (Time.time - start < timeToStop)
        //    {
        //        float part = (Time.time - start) / timeToStop;
        //        transform.position = Vector3.Lerp(transform.position, lastWisp, part * force);
        //        yield return null;
        //    }
        //}

    }
}