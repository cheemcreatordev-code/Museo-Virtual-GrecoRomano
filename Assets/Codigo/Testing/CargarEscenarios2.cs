using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscenarios2 : MonoBehaviour
{
    public int[] escenarios;

    void Start()
    {
        foreach (int indice in escenarios)
        {
            if (!SceneManager.GetSceneByBuildIndex(indice).isLoaded)
            {
                SceneManager.LoadScene(indice, LoadSceneMode.Additive);
            }
        }
    }
}