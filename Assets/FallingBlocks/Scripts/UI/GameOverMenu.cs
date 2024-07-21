using UnityEngine;
using UnityEngine.SceneManagement;

namespace FallingBlocks.Menu
{
    public class GameOverMenu : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 0f;
        }

        public void NewGame()
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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
