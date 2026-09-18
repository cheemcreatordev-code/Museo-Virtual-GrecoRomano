using UnityEngine;
using UnityEngine.Profiling;
using System.Diagnostics;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class F3Debuger : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Frecuencia de actualización para los FPS y la CPU (en segundos)")]
    [SerializeField] private float updateInterval = 0.5f;

    [Header("Referencias")]
    [Tooltip("Asigna la cámara principal o la cámara del jugador")]
    [SerializeField] private Transform playerTransform;

    private TextMeshProUGUI debugText;
    private bool isVisible = false;

    // Métricas dinámicas
    private float fpsAccumulator = 0f;
    private int fpsFrames = 0;
    private float fpsLeftTime;
    private int currentFps = 0;
    private long allocatedRamMB = 0;

    // Cálculo de porcentaje de CPU
    private Process currentProcess;
    private System.TimeSpan lastCpuTime;
    private System.DateTime lastSampleTime;
    private float cpuUsagePercent = 0f;
    private int processorCount = 1;

    // Caché de hardware estático
    private string cpuName;
    private string gpuName;
    private int vramMB;

    // Buffer de caracteres (Cero GC Alloc)
    private readonly char[] textBuffer = new char[512];
    private int bufferLength = 0;

    private void Awake()
    {
        debugText = GetComponent<TextMeshProUGUI>();

        if (playerTransform == null && Camera.main != null)
        {
            playerTransform = Camera.main.transform;
        }

        fpsLeftTime = updateInterval;

        // Caché de hardware
        cpuName = SystemInfo.processorType;
        gpuName = SystemInfo.graphicsDeviceName;
        vramMB = SystemInfo.graphicsMemorySize;
        processorCount = SystemInfo.processorCount;

        // Inicializar lectura de proceso para la CPU
        try
        {
            currentProcess = Process.GetCurrentProcess();
            lastCpuTime = currentProcess.TotalProcessorTime;
            lastSampleTime = System.DateTime.UtcNow;
        }
        catch
        {
            UnityEngine.Debug.LogWarning("F3Debuger: No se pudo acceder a las métricas del proceso del sistema.");
        }

        debugText.enabled = isVisible;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            isVisible = !isVisible;
            debugText.enabled = isVisible;
        }

        if (!isVisible || playerTransform == null) return;

        // --- Lectura de RAM constante (Cada Frame) ---
        allocatedRamMB = Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024);

        // --- Muestreo de FPS y CPU por Intervalos ---
        fpsAccumulator += Time.timeScale / Time.unscaledDeltaTime;
        fpsFrames++;
        fpsLeftTime -= Time.unscaledDeltaTime;

        if (fpsLeftTime <= 0.0f)
        {
            currentFps = Mathf.RoundToInt(fpsAccumulator / fpsFrames);

            // Calcular porcentaje de CPU en el intervalo
            CalculateCpuUsage();

            fpsAccumulator = 0.0f;
            fpsFrames = 0;
            fpsLeftTime = updateInterval;
        }

        RenderText();
    }

    private void CalculateCpuUsage()
    {
        if (currentProcess == null) return;

        currentProcess.Refresh();
        System.DateTime now = System.DateTime.UtcNow;
        System.TimeSpan cpuTime = currentProcess.TotalProcessorTime;

        double timeWindow = (now - lastSampleTime).TotalMilliseconds;
        double cpuTimeUsed = (cpuTime - lastCpuTime).TotalMilliseconds;

        if (timeWindow > 0)
        {
            cpuUsagePercent = (float)((cpuTimeUsed / (timeWindow * processorCount)) * 100.0);
            cpuUsagePercent = Mathf.Clamp(cpuUsagePercent, 0f, 100f);
        }

        lastCpuTime = cpuTime;
        lastSampleTime = now;
    }

    private void RenderText()
    {
        Vector3 pos = playerTransform.position;
        bufferLength = 0;

        // --- FPS y Posición ---
        AppendString("FPS: ");
        AppendInt(currentFps);
        AppendString("\nXYZ: ");
        AppendFloat(pos.x);
        AppendString(" / ");
        AppendFloat(pos.y);
        AppendString(" / ");
        AppendFloat(pos.z);

        // --- Memoria RAM (En tiempo real) ---
        AppendString("\nRAM: ");
        AppendInt((int)allocatedRamMB);
        AppendString(" MB");

        // --- Procesador (CPU %) ---
        AppendString("\nCPU: ");
        AppendString(cpuName);
        AppendString(" [Uso: ");
        AppendFloat(cpuUsagePercent);
        AppendString("%]");

        // --- Tarjeta Gráfica (GPU) ---
        AppendString("\nGPU: ");
        AppendString(gpuName);
        AppendString(" [VRAM: ");
        AppendInt(vramMB);
        AppendString(" MB]");

        // Renderizado optimizado sin GC
        debugText.SetCharArray(textBuffer, 0, bufferLength);
    }

    // --- Métodos Auxiliares Cero GC ---

    private void AppendString(string value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (bufferLength < textBuffer.Length)
                textBuffer[bufferLength++] = value[i];
        }
    }

    private void AppendInt(int value)
    {
        if (value == 0)
        {
            if (bufferLength < textBuffer.Length) textBuffer[bufferLength++] = '0';
            return;
        }

        if (value < 0)
        {
            if (bufferLength < textBuffer.Length) textBuffer[bufferLength++] = '-';
            value = -value;
        }

        int temp = value;
        int digits = 0;
        while (temp > 0) { digits++; temp /= 10; }

        for (int i = digits - 1; i >= 0; i--)
        {
            int digit = value % 10;
            if (bufferLength + i < textBuffer.Length)
                textBuffer[bufferLength + i] = (char)('0' + digit);
            value /= 10;
        }

        bufferLength += digits;
    }

    private void AppendFloat(float value)
    {
        AppendInt((int)value);
        if (bufferLength < textBuffer.Length) textBuffer[bufferLength++] = '.';

        int decimals = Mathf.Abs((int)((value - (int)value) * 100));
        if (decimals < 10 && bufferLength < textBuffer.Length)
            textBuffer[bufferLength++] = '0';

        AppendInt(decimals);
    }
}