using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        public void LoadScene(string levelIndex) => SceneManager.LoadScene(levelIndex);
    }
}
