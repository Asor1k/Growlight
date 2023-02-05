using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jam
{
    public class DarknessSpawner : MonoBehaviour
    {
        [SerializeField] private DarkBuddy darkBuddyPrefab;
        [SerializeField] private Transform spawnerParent;
        [SerializeField] private SpawnerConfig spawnerConfig;
        [SerializeField] private RootControl rootControl;

        private List<SpawnerNode> spawners;
        private Camera cam;

        private float modifier = 1;


        private void Start()
        {
            spawners = spawnerParent.GetComponentsInChildren<SpawnerNode>().ToList();
            cam = Camera.main;
            StartCoroutine(Spawning());
        }

        private IEnumerator Spawning()
        {
            float startTime = 0;
            while (startTime < 10f)
            {
                if (rootControl.CanGrow())
                {
                    startTime += Time.deltaTime;
                }

                yield return null;
            }

            
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(spawnerConfig.SpawnFrequencyMinimum * modifier,
                    spawnerConfig.SpawnFrequencyMaximum * modifier));
                int randomIndex = Random.Range(0, spawners.Count);
                DarkBuddy darkBuddy = Instantiate(darkBuddyPrefab, spawners[randomIndex].transform.position, Quaternion.identity, null);

                darkBuddy.Init(spawners[randomIndex].GetShootingDirection());
            }
        }

        public void IncreaseDifficulty()
        {
            modifier = 0.5f;
        }
    }
}