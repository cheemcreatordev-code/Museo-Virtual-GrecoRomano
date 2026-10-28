
using UnityEngine;
using UnityEngine.Playables;

public class PausarCinematicas : MonoBehaviour
{
    private PlayableDirector[] cinematicas;
    private bool estaPausado = false;

    private void Awake()
    {
        BuscarCinematicas();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CambiarPausa();
        }
    }

    private void BuscarCinematicas()
    {
        cinematicas = FindObjectsOfType<PlayableDirector>(true);
    }

    private void CambiarPausa()
    {
        estaPausado = !estaPausado;

        foreach (PlayableDirector cinematica in cinematicas)
        {
            if (cinematica == null)
                continue;

            if (estaPausado)
            {
                if (cinematica.state == PlayState.Playing)
                {
                    cinematica.Pause();
                }
            }
            else
            {
                if (cinematica.state == PlayState.Paused)
                {
                    cinematica.Resume();
                }
            }
        }
    }
}