using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuSystem
{
    public class MenuController : MonoBehaviour
    {
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}
