
using UnityEngine;

public class MirarAlPlayer : MonoBehaviour
{
    [Header("Objetivo")]
    [SerializeField] private Transform jugador;

    [Header("Configuración")]
    [SerializeField] private float velocidadRotacion = 50f;

    private void Update()
    {
        if (jugador == null)
            return;

        MirarJugador();
    }

    private void MirarJugador()
    {
        Vector3 direccion = jugador.position - transform.position;

        // Impide que mire hacia arriba o hacia abajo.
        direccion.y = 0f;

        if (direccion.sqrMagnitude < 0.001f)
            return;

        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);

        // Solo modifica la rotación horizontal.
        Vector3 rotacionActual = transform.eulerAngles;
        float anguloObjetivo = rotacionObjetivo.eulerAngles.y;

        float nuevoAngulo = Mathf.MoveTowardsAngle(
            rotacionActual.y,
            anguloObjetivo,
            velocidadRotacion * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(
            rotacionActual.x,
            nuevoAngulo,
            rotacionActual.z
        );
    }
}