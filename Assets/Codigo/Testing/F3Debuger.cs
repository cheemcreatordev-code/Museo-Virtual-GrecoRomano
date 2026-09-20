using UnityEngine;
using System.Diagnostics;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class F3Debuger : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float updateInterval = 0.5f;

    [Header("Referencias")]
    [SerializeField] private Transform playerTransform;

    private TextMeshProUGUI debugText;
    private bool isVisible = false;

    // FPS
    private float fpsAccumulator = 0f;
    private int fpsFrames = 0;
    private float fpsLeftTime;
    private int currentFps = 0;

    // RAM
    private long ramUsageMB = 0;

    // CPU
    private Process currentProcess;
    private System.TimeSpan lastCpuTime;
    private System.DateTime lastSampleTime;
    private float cpuUsagePercent = 0f;
    private int processorCount = 1;

    // Hardware
    private string cpuName;
    private string gpuName;
    private int vramMB;

    // Buffer
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

        // Hardware
        cpuName = SystemInfo.processorType;
        gpuName = SystemInfo.graphicsDeviceName;
        vramMB = SystemInfo.graphicsMemorySize;
        processorCount = SystemInfo.processorCount;

        // Proceso actual
        try
        {
            currentProcess = Process.GetCurrentProcess();

            lastCpuTime = currentProcess.TotalProcessorTime;
            lastSampleTime = System.DateTime.UtcNow;

            UpdateRam();
        }
        catch
        {
            currentProcess = null;
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

        if (!isVisible || playerTransform == null)
            return;

        // RAM actualizada cada frame
        UpdateRam();

        // FPS
        fpsAccumulator += Time.timeScale / Time.unscaledDeltaTime;
        fpsFrames++;

        fpsLeftTime -= Time.unscaledDeltaTime;

        if (fpsLeftTime <= 0f)
        {
            currentFps = Mathf.RoundToInt(
                fpsAccumulator / fpsFrames
            );

            CalculateCpuUsage();

            fpsAccumulator = 0f;
            fpsFrames = 0;
            fpsLeftTime = updateInterval;
        }

        RenderText();
    }

    private void UpdateRam()
    {
        if (currentProcess == null)
            return;

        try
        {
            currentProcess.Refresh();

            // Memoria física actualmente residente
            long workingSet = currentProcess.WorkingSet64;

            // Memoria privada del proceso
            long privateMemory = currentProcess.PrivateMemorySize64;

            // Usar Working Set si es válido
            if (workingSet > 0)
            {
                ramUsageMB = workingSet / (1024 * 1024);
            }
            else if (privateMemory > 0)
            {
                ramUsageMB = privateMemory / (1024 * 1024);
            }
        }
        catch
        {
            // Mantener el último valor válido
        }
    }

    private void CalculateCpuUsage()
    {
        if (currentProcess == null)
            return;

        try
        {
            currentProcess.Refresh();

            System.DateTime now = System.DateTime.UtcNow;
            System.TimeSpan cpuTime =
                currentProcess.TotalProcessorTime;

            double timeWindow =
                (now - lastSampleTime).TotalMilliseconds;

            double cpuTimeUsed =
                (cpuTime - lastCpuTime).TotalMilliseconds;

            if (timeWindow > 0)
            {
                cpuUsagePercent =
                    (float)(
                        (cpuTimeUsed /
                        (timeWindow * processorCount)) * 100.0
                    );

                cpuUsagePercent =
                    Mathf.Clamp(cpuUsagePercent, 0f, 100f);
            }

            lastCpuTime = cpuTime;
            lastSampleTime = now;
        }
        catch
        {
        }
    }

    private void RenderText()
    {
        Vector3 pos = playerTransform.position;
        bufferLength = 0;

        // FPS
        AppendString("FPS: ");
        AppendInt(currentFps);

        // Posición
        AppendString("\nXYZ: ");
        AppendFloat(pos.x);
        AppendString(" / ");
        AppendFloat(pos.y);
        AppendString(" / ");
        AppendFloat(pos.z);

        // RAM
        AppendString("\nRAM: ");
        AppendInt((int)ramUsageMB);
        AppendString(" MB");

        // CPU
        AppendString("\nCPU: ");
        AppendString(cpuName);
        AppendString(" [Uso: ");
        AppendFloat(cpuUsagePercent);
        AppendString("%]");

        // GPU
        AppendString("\nGPU: ");
        AppendString(gpuName);
        AppendString(" [VRAM: ");
        AppendInt(vramMB);
        AppendString(" MB]");

        debugText.SetCharArray(
            textBuffer,
            0,
            bufferLength
        );
    }

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
            if (bufferLength < textBuffer.Length)
                textBuffer[bufferLength++] = '0';

            return;
        }

        if (value < 0)
        {
            if (bufferLength < textBuffer.Length)
                textBuffer[bufferLength++] = '-';

            value = -value;
        }

        int temp = value;
        int digits = 0;

        while (temp > 0)
        {
            digits++;
            temp /= 10;
        }

        for (int i = digits - 1; i >= 0; i--)
        {
            int digit = value % 10;

            if (bufferLength + i < textBuffer.Length)
                textBuffer[bufferLength + i] =
                    (char)('0' + digit);

            value /= 10;
        }

        bufferLength += digits;
    }

    private void AppendFloat(float value)
    {
        int integerPart = (int)value;

        AppendInt(integerPart);

        if (bufferLength < textBuffer.Length)
            textBuffer[bufferLength++] = '.';

        int decimals = Mathf.Abs(
            (int)((value - integerPart) * 100)
        );

        if (decimals < 10 &&
            bufferLength < textBuffer.Length)
        {
            textBuffer[bufferLength++] = '0';
        }

        AppendInt(decimals);
    }
}