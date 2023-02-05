using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    public class Wisp : MonoBehaviour
    {


        public bool IsPressing => isPressing;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform background;
        [SerializeField] private CameraBehaviour cameraBehaviour;
        [SerializeField] private WispLight wispLight;
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] private float growSpeed;
        [SerializeField] private RootControl rootControl;

        private int layerMask = 3;

        private int invertedMask;

        private bool isPressing;


        private void Start()
        {
            invertedMask = 1 << layerMask;
            //invertedMask = ~invertedMask;
        }

        public WispLight GetWispLight()
        {
            return wispLight;
        }

        private void Update()
        {
            //   sphereCollider.radius += Time.deltaTime / 2;
            if (rootControl.CanGrow())
            {
                transform.localScale += Vector3.one * growSpeed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }

            if (Input.GetMouseButtonDown(0))
            {
                transform.SetParent(null); //wispLight.StartAttracting(transform);
                wispLight.StartDragging(sphereCollider);
            }

            if (Input.GetMouseButton(0))
            {
                //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //transform.position = mousePosition;
                //Vector2 pointPos = Camera.mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Vector3.Distance(mainCamera.transform.position, background.position)));
                //print(pointPos);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Vector3 pointPos = Vector3.one;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, invertedMask))
                {
                    pointPos = hit.point;
                    transform.position = new Vector3(pointPos.x, pointPos.y, pointPos.z);
                    wispLight.Drag(sphereCollider);
                    isPressing = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                transform.SetParent(mainCamera.transform);
                isPressing = false;
                wispLight.EndDragging();
            }
        }
    }
}