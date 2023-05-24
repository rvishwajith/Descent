using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PerformanceMetrics : MonoBehaviour
{
    [Header("Label (TextMeshPro)")]
    public TextMeshProUGUI label;

    FrameRateCalculator calculator = null;

    void Start()
    {
        Application.targetFrameRate = 120;
        calculator = new();
    }

    void Update()
    {
        calculator.Update(Time.time);
    }

    void OnGUI()
    {
        label.text = calculator.ContextualFPS();
    }
}

class FrameRateCalculator
{
    private Queue<float> times;
    private int maxSamples = 400, minSamples = 60;
    private int maxDecimals = 2;

    public FrameRateCalculator()
    {
        times = new();
    }

    public void Update(float deltaT)
    {
        times.Enqueue(deltaT);
        while (times.Count > maxSamples)
        {
            times.Dequeue();
        }
    }

    public float AverageFPS()
    {
        float numSamples = times.Count;
        float sumDeltaT = 0;
        foreach (float deltaT in times)
        {
            sumDeltaT += deltaT;
        }
        float meanDeltaT = sumDeltaT / numSamples;
        float meanFPS = 1 / meanDeltaT;
        return meanFPS;
    }

    public string ContextualFPS()
    {
        if (times.Count < minSamples)
        {
            return "... FPS";
        }
        int castScale = (int)Mathf.Pow(10, maxDecimals);
        int fpsScaled = (int)(AverageFPS() * castScale);
        float fpsRounded = (float)(fpsScaled / castScale);
        return fpsRounded + " FPS";
    }
}