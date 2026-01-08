using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelSystem
{
    public class LevelManager : MonoBehaviour
    {
        public void LoadScene(int levelIndex) => SceneManager.LoadScene(levelIndex);
    }
}
