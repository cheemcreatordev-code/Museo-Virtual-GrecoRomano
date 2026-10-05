using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenario : MonoBehaviour
{
    private string numeros = "";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) AgregarNumero("0");
        if (Input.GetKeyDown(KeyCode.Alpha1)) AgregarNumero("1");
        if (Input.GetKeyDown(KeyCode.Alpha2)) AgregarNumero("2");
        if (Input.GetKeyDown(KeyCode.Alpha3)) AgregarNumero("3");
        if (Input.GetKeyDown(KeyCode.Alpha4)) AgregarNumero("4");
        if (Input.GetKeyDown(KeyCode.Alpha5)) AgregarNumero("5");
        if (Input.GetKeyDown(KeyCode.Alpha6)) AgregarNumero("6");
        if (Input.GetKeyDown(KeyCode.Alpha7)) AgregarNumero("7");
        if (Input.GetKeyDown(KeyCode.Alpha8)) AgregarNumero("8");
        if (Input.GetKeyDown(KeyCode.Alpha9)) AgregarNumero("9");
    }

    void AgregarNumero(string numero)
    {
        numeros += numero;

        if (numeros.Length >= 2)
        {
            switch (numeros)
            {
                // INICIO
                case "00":
                    SceneManager.LoadScene(0);
                    break;

                // PARTE 1 DEL INICIO
                case "01":
                    SceneManager.LoadScene(1);
                    break;

                // PISOS PRINCIPALES
                case "02":
                    SceneManager.LoadScene(4);
                    break;

                case "03":
                    SceneManager.LoadScene(5);
                    break;

                case "04":
                    SceneManager.LoadScene(6);
                    break;

                case "05":
                    SceneManager.LoadScene(7);
                    break;

                case "06":
                    SceneManager.LoadScene(8);
                    break;
            }

            numeros = "";
        }
    }
}