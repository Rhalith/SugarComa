using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Menus.MainMenu.Scripts
{
    public class MainMenuController : MonoBehaviour
    {
        public void Play()
        {
            // Get next scene according to build index
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}