using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FallingBlocks.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void Resume()
        {
            this.gameObject.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
