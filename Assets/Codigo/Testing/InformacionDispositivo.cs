using System;
using System.IO;
using System.Text;
using UnityEngine;

public class InformacionDispositivo : MonoBehaviour
{
    [Header("Archivo")]
    public string NombreArchivo = "InformacionDispositivo.txt";

    private void Start()
    {
        CrearArchivo();
    }

    void CrearArchivo()
    {
        StringBuilder Texto = new StringBuilder();

        Texto.AppendLine("========================================");
        Texto.AppendLine("       INFORMACION DEL DISPOSITIVO");
        Texto.AppendLine("========================================");
        Texto.AppendLine();

        // Juego
        Texto.AppendLine("----- JUEGO -----");
        Texto.AppendLine("Nombre del juego: " + Application.productName);
        Texto.AppendLine("Version del juego: " + Application.version);
        Texto.AppendLine("Version de Unity: " + Application.unityVersion);
        Texto.AppendLine();

        // Sistema operativo
        Texto.AppendLine("----- SISTEMA OPERATIVO -----");
        Texto.AppendLine("Sistema operativo: " + SystemInfo.operatingSystem);
        Texto.AppendLine("Plataforma: " + Application.platform);
        Texto.AppendLine();

        // CPU
        Texto.AppendLine("----- CPU -----");
        Texto.AppendLine("Procesador: " + SystemInfo.processorType);
        Texto.AppendLine("Nucleos: " + SystemInfo.processorCount);
        Texto.AppendLine("Frecuencia CPU: " + SystemInfo.processorFrequency + " MHz");
        Texto.AppendLine();

        // RAM
        Texto.AppendLine("----- MEMORIA RAM -----");
        Texto.AppendLine("RAM disponible: " + SystemInfo.systemMemorySize + " MB");
        Texto.AppendLine();

        // GPU
        Texto.AppendLine("----- GPU -----");
        Texto.AppendLine("GPU: " + SystemInfo.graphicsDeviceName);
        Texto.AppendLine("Fabricante GPU: " + SystemInfo.graphicsDeviceVendor);
        Texto.AppendLine("Memoria GPU: " + SystemInfo.graphicsMemorySize + " MB");
        Texto.AppendLine("API grafica: " + SystemInfo.graphicsDeviceType);
        Texto.AppendLine();

        // Pantalla
        Texto.AppendLine("----- PANTALLA -----");
        Texto.AppendLine("Resolucion: " + Screen.width + "x" + Screen.height);
        Texto.AppendLine("DPI: " + Screen.dpi);
        Texto.AppendLine("Orientacion: " + Screen.orientation);
        Texto.AppendLine();

        // Dispositivo
        Texto.AppendLine("----- DISPOSITIVO -----");
        Texto.AppendLine("Modelo: " + SystemInfo.deviceModel);
        Texto.AppendLine("Nombre del dispositivo: " + SystemInfo.deviceName);
        Texto.AppendLine("Tipo: " + SystemInfo.deviceType);
        Texto.AppendLine();

#if UNITY_ANDROID
        Texto.AppendLine("----- ANDROID -----");
        Texto.AppendLine("Version Android: " + SystemInfo.operatingSystem);
        Texto.AppendLine("Modelo Android: " + SystemInfo.deviceModel);
        Texto.AppendLine("Identificador de dispositivo: " + SystemInfo.deviceUniqueIdentifier);
        Texto.AppendLine();
#endif

#if UNITY_STANDALONE_WIN
        Texto.AppendLine("----- WINDOWS -----");
        Texto.AppendLine("Arquitectura: " + (Environment.Is64BitOperatingSystem ? "64 bits" : "32 bits"));
        Texto.AppendLine("Version Windows: " + SystemInfo.operatingSystem);
        Texto.AppendLine();
#endif

        // Fecha
        Texto.AppendLine("----- INFORMACION DEL REGISTRO -----");
        Texto.AppendLine("Fecha: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Texto.AppendLine();

        Texto.AppendLine("========================================");
        Texto.AppendLine("              FIN DEL INFORME");
        Texto.AppendLine("========================================");

        // Determinar carpeta del juego
        string Ruta;

#if UNITY_ANDROID
        // En Android no se puede escribir directamente en la carpeta APK.
        // Application.persistentDataPath es la carpeta correcta para archivos del juego.
        Ruta = Application.persistentDataPath;
#else
        // En Windows intenta colocar el TXT junto al .exe.
        Ruta = AppDomain.CurrentDomain.BaseDirectory;
#endif

        string RutaArchivo = Path.Combine(Ruta, NombreArchivo);

        try
        {
            File.WriteAllText(RutaArchivo, Texto.ToString(), Encoding.UTF8);

            Debug.Log("Informacion del dispositivo guardada en:");
            Debug.Log(RutaArchivo);
        }
        catch (Exception Error)
        {
            Debug.LogError("No se pudo crear el archivo: " + Error.Message);
        }
    }
}