using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Jam
{
    public class RootControl : MonoBehaviour
    {
        [SerializeField] private int maximumBranches;
        [SerializeField] private Wisp wisp;
        [SerializeField] private int amountBreakToDie;
        [SerializeField] private int amountToWin;
        [SerializeField] private DarknessSpawner darknessSpawner;

        [SerializeField] private PlayableDirector winCutscene;
        [SerializeField] private Transform looseImage;
        [SerializeField] private Image rootImage;

        private int branchAmount;
        private int brokenAmount;

        private List<DarkBuddy> stickedBuddies = new List<DarkBuddy>();

        private void Start()
        {
            AudioManager.Instance.CallFadeIn(1f);
            AudioManager.Instance.PlayMusic(Audio.GameAmbient);
        }

        public bool CanGrow()
        {
            return stickedBuddies.Count == 0 && wisp.IsPressing;
        }

        public bool CanSpawn()
        {
            return Random.value+0.1f > (float)branchAmount / maximumBranches;
        }

        public void AddBranch()
        {
            branchAmount++;
            rootImage.fillAmount = (float)branchAmount / maximumBranches;
            if (branchAmount > maximumBranches / 2)
            {
                darknessSpawner.IncreaseDifficulty();
            }
            if (branchAmount >= amountToWin + 1)
            {
                Win();
            }
        }

        public void RemoveBranch()
        {
            branchAmount--;
            rootImage.fillAmount = (float)branchAmount / maximumBranches;
        }

        private IEnumerator LateLoose()
        {
            yield return new WaitForSeconds(1f);
            looseImage.gameObject.SetActive(true);
        }

        private void Win()
        {
            if (wisp.GetWispLight())
            {
                Destroy(wisp.GetWispLight().gameObject);
            }
            AudioManager.Instance.CallFadeIn(1f);
            AudioManager.Instance.PlayMusic(Audio.Intro);
            winCutscene.Play();
        }

        private void Loose()
        {
            if (wisp.GetWispLight())
            {
                Destroy(wisp.GetWispLight().gameObject);
            }
            AudioManager.Instance.CallFadeIn(1f);
            AudioManager.Instance.PlayMusic(Audio.Intro);
            //looseImage.gameObject.SetActive(true);
            StartCoroutine(LateLoose());
        }

        public void AddBrokenBranch()
        {
            brokenAmount++;
            RemoveBranch();
            if (brokenAmount >= amountBreakToDie)
            {
                Loose();
            }
        }


        public void AddSticked(DarkBuddy darkBuddy)
        {
            stickedBuddies.Add(darkBuddy);
        }
        

        public void RemoveSticked(DarkBuddy darkBuddy)
        {
            stickedBuddies.RemoveAll(buddie => buddie == null);
            if (darkBuddy)
            {
                stickedBuddies.RemoveAll(buddie => buddie.Equals(darkBuddy));
            }
        }
    }
}