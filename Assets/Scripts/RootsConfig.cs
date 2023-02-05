using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jam
{
    [CreateAssetMenu(fileName = "RootsConfig", menuName = "Roots Config", order = 0)]
    public class RootsConfig : ScriptableObject
    {
        public int MaximumBranches => maximumBranches;
        public float MinimumDelay => minimumDelay;
        public float MaximumDelay => maximumDelay;


        [SerializeField] private int maximumBranches;
        [SerializeField] private float minimumDelay;
        [SerializeField] private float maximumDelay;
    }
}