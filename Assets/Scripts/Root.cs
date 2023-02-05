using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jam
{
    public class Root : MonoBehaviour
    {

        [SerializeField] private float speed;
        [SerializeField] private MeshFilter mesh;
        [SerializeField] private Transform pointsHolder;
        [SerializeField] private RootsConfig config;
        [SerializeField] private Root rootPrefab;
        [SerializeField] private RootControl _rootControl;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private List<Material> _materials;
        [SerializeField] private Rigidbody _rb;


        private List<Transform> points;
        private List<Vector3> vertices;
        private int amountOfBranches;
        private Transform root;
        private Vector3 offset;
        private List<DarkBuddy> stickedDarkBuddies = new List<DarkBuddy>();
        private bool isBroken;
        private List<Root> childRoots = new List<Root>();


        private void Start()
        {
            points = pointsHolder.GetComponentsInChildren<Transform>().ToList();
            mesh.transform.localScale = Vector3.one;
            amountOfBranches = 0;
            StartCoroutine(SpawningBranches());
        }

        public void SetRootMaximum(RootControl rtmc)
        {
            _rootControl = rtmc;
        }

        private IEnumerator DelayDestroy()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
            for (int i = 0; i < stickedDarkBuddies.Count; i++)
            {
                if (stickedDarkBuddies[i] != null)
                {
                    Destroy(stickedDarkBuddies[i].gameObject);
                }
            }
            _rootControl.RemoveSticked(null);
        }

        public void BreakDown()
        {
            AudioManager.Instance.PlayEffect(Audio.BranchBreak);
            _rootControl.AddBrokenBranch();
            _rb.isKinematic = false;
            stickedDarkBuddies.ForEach(buddie => _rootControl.RemoveSticked(buddie));
            isBroken = true;
            childRoots.ForEach(root => root.BreakDown());
            //print();
            StartCoroutine(DelayDestroy());
        }

        private void Darken()
        {
            _meshRenderer.material = _materials[Mathf.Clamp(stickedDarkBuddies.Count, 0, 4)];
        }

        private void LightenUp()
        {
            _meshRenderer.material = _materials[Mathf.Clamp(stickedDarkBuddies.Count, 0, 4)];
        }

        public void SetMeshRotation(Quaternion quaternion)
        {
            mesh.transform.rotation = quaternion;
        }

        public void SetRoot(Transform root)
        {
            offset = transform.position - root.position;
            this.root = root;
        }

        public void Stick(DarkBuddy darkBuddy)
        {
            stickedDarkBuddies.Add(darkBuddy);
            _rootControl.AddSticked(darkBuddy);
            Darken();
            if (stickedDarkBuddies.Count == 5)
            {
                BreakDown();
            }
        }

        public void UnStick(DarkBuddy darkBuddy)
        {
            stickedDarkBuddies.RemoveAll(buddie => darkBuddy.Equals(buddie));
            LightenUp();

            _rootControl.RemoveSticked(darkBuddy);

        }

        public Transform GetClosestPoint(Vector3 pos)
        {
            Transform closest = points[0];
            float minDistance = Vector3.Distance(pos, closest.position);
            foreach (var point in points)
            {
                if (Vector3.Distance(pos, point.position) < minDistance)
                {
                    minDistance = Vector3.Distance(pos, point.position);
                    closest = point;
                }
            }
            return closest;
        }


        private IEnumerator SpawningBranches()
        {
            while (amountOfBranches < config.MaximumBranches)
            {
                yield return new WaitForSeconds(Random.Range(config.MinimumDelay, config.MaximumDelay));
                
                if(!_rootControl.CanSpawn() || !_rootControl.CanGrow())continue;
                
                int randomIndex = Random.Range(0, points.Count);
                bool sideLeft = Random.value > 0.5;
                Quaternion randomRotation =
                    Quaternion.Euler(Random.Range(-90,-15), sideLeft?90:-90 ,0);
                Root root = Instantiate(rootPrefab, points[randomIndex].position, Quaternion.identity); 
                root.SetMeshRotation(randomRotation);
              //  root.transform.SetParent(points[randomIndex]);
                root.SetRoot(points[randomIndex]);
                root.SetRootMaximum(_rootControl);
                childRoots.Add(root);
                amountOfBranches++;
                _rootControl.AddBranch();
            }
        }


        private void Update()
        {
      
            if (!_rootControl.CanGrow()) return;
            //vertices = mesh.sharedMesh.vertices.ToList();
            //Matrix4x4 localToWorld = transform.localToWorldMatrix;
            //List<Vector3> worldVerticies = new List<Vector3>();
            //for (int i = 0; i < vertices.Count; ++i)
            //{
            //    Vector3 world_v = localToWorld.MultiplyPoint3x4(vertices[i]);
            //    worldVerticies.Add(world_v);
            //    print(world_v);
            //}
            ////print(worldVerticies.Last
            if (root)
            {
                transform.position = root.position + offset;

            }
            mesh.transform.localScale += Vector3.one * speed * Time.deltaTime;

        }
    }
}
