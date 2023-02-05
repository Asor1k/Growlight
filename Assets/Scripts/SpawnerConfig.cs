using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    [CreateAssetMenu()]
    public class SpawnerConfig : ScriptableObject
    {
        public float SpawnFrequencyMinimum => spawnFrequencyMinimum;

        public float SpawnFrequencyMaximum => spawnFrequencyMaximum;

        [SerializeField] private float spawnFrequencyMinimum;

        [SerializeField] private float spawnFrequencyMaximum;

    }
}