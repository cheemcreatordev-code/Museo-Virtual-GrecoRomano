using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscenario : MonoBehaviour
{
    void Start()
    {
        if (!SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }

        if (!SceneManager.GetSceneByBuildIndex(2).isLoaded)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
    }
}