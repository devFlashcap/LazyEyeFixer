using UnityEngine;
using UnityEngine.SceneManagement;

namespace FallingBlocks.Menu
{
    public class GameOverMenu : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void NewGame()
        {
            SceneManager.LoadScene("FallingBlocks", LoadSceneMode.Single);
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
