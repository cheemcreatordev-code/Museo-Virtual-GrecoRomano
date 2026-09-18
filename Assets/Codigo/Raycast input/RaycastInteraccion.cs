using UnityEngine;
using UnityEngine.SceneManagement;

public class RaycastInteraccion : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera camara;

    [Header("Raycast")]
    [SerializeField] private float distancia = 3f;
    [SerializeField] private LayerMask capasDetectables;

    [Header("Detección")]
    [SerializeField] private string tagObjetivo = "Interactuable";
    [SerializeField] private KeyCode input = KeyCode.E;

    // Objeto actualmente detectado
    private GameObject objetoDetectado;

    private void Update()
    {
        DetectarObjeto();

        if (objetoDetectado != null && Input.GetKeyDown(input))
        {
            FuncionAlDetectar(objetoDetectado);
        }
    }

    private void DetectarObjeto()
    {
        Ray ray = camara.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, distancia, capasDetectables))
        {
            GameObject objeto = hit.collider.gameObject;

            if (objeto.CompareTag(tagObjetivo))
            {
                // Solo actualizamos la referencia si cambió el objeto.
                if (objetoDetectado != objeto)
                {
                    objetoDetectado = objeto;
                    AlDetectarObjeto(objetoDetectado);
                }

                return;
            }
        }

        // Ya no hay ningún objeto válido en el centro.
        if (objetoDetectado != null)
        {
            objetoDetectado = null;
            AlDejarDeDetectar();
        }
    }

    private void AlDetectarObjeto(GameObject objeto)
    {
        Debug.Log("Detectando: " + objeto.name);

        // Aquí puedes mostrar:
        // "E - Interactuar"
    }

    private void AlDejarDeDetectar()
    {
        Debug.Log("Ya no se detecta ningún objeto.");

        // Aquí puedes ocultar:
        // "E - Interactuar"
    }

    private void FuncionAlDetectar(GameObject objeto)
    {
        Debug.Log("Interacción con: " + objeto.name);

        // AQUÍ VA LA FUNCIÓN DE INTERACCIÓN.
        SceneManager.LoadScene(1);
    }

    private void OnDrawGizmosSelected()
    {
        if (camara == null)
            return;

        Ray ray = camara.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Gizmos.DrawRay(ray.origin, ray.direction * distancia);
    }
}