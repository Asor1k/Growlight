using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Jam
{
    public class SceneController : MonoBehaviour
    {


        public void SwitchScene(string sceneName)
        {
            StartCoroutine(Switching(sceneName));
        }

        private IEnumerator Switching(string sceneName)
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(sceneName);
        }


        public void QuitGame()
        {
            Application.Quit();
        }

    }
}