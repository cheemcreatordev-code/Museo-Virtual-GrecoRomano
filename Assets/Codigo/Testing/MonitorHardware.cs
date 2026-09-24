using UnityEngine;
using TMPro;
using System;

public class MonitorHardware : MonoBehaviour
{
    [Header("TextMeshPro UI")]
    public TextMeshProUGUI textoRAM;
    public TextMeshProUGUI textoCPU;
    public TextMeshProUGUI textoGPU;
    public TextMeshProUGUI textoFPS;

    [Header("Configuración")]
    public float intervaloActualizacion = 0.5f;

    private float temporizador;
    private float fpsPromedio;

    // Variables para calcular FPS correctamente
    private int frames;
    private float tiempoFPS;

    void Start()
    {
        ActualizarDatos();
    }

    void Update()
    {
        // Contar frames
        frames++;
        tiempoFPS += Time.unscaledDeltaTime;

        // Calcular FPS promedio durante el intervalo
        if (tiempoFPS >= intervaloActualizacion)
        {
            fpsPromedio = frames / tiempoFPS;

            frames = 0;
            tiempoFPS = 0f;

            ActualizarDatos();
        }
    }

    void ActualizarDatos()
    {
        ActualizarRAM();
        ActualizarCPU();
        ActualizarGPU();
        ActualizarFPS();
    }

    void ActualizarRAM()
    {
        // RAM total del dispositivo
        float ramTotalMB = SystemInfo.systemMemorySize;

        // Memoria que Unity tiene actualmente asignada
        long memoriaUnityBytes = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();

        float memoriaUnityMB =
            memoriaUnityBytes / 1024f / 1024f;

        float porcentaje =
            (memoriaUnityMB / ramTotalMB) * 100f;

        textoRAM.text =
            "RAM: " +
            memoriaUnityMB.ToString("0.0") +
            " MB / " +
            ramTotalMB.ToString("0") +
            " MB (" +
            porcentaje.ToString("0.0") +
            "%)";
    }

    void ActualizarCPU()
    {
        textoCPU.text =
            "CPU: " +
            SystemInfo.processorType +
            " | Núcleos: " +
            SystemInfo.processorCount;
    }

    void ActualizarGPU()
    {
        textoGPU.text =
            "GPU: " +
            SystemInfo.graphicsDeviceName +
            " | VRAM: " +
            SystemInfo.graphicsMemorySize +
            " MB";
    }

    void ActualizarFPS()
    {
        textoFPS.text =
            "FPS: " +
            fpsPromedio.ToString("0");
    }
}