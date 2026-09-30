using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscenario : MonoBehaviour
{
    void Start()
    {
        if (!SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }

        if (!SceneManager.GetSceneByBuildIndex(3).isLoaded)
        {
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
        }
    }
} 