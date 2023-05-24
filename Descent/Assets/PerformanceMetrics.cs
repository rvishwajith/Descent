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
        // Application.targetFrameRate = 120;
        calculator = new();
    }

    void Update()
    {
        calculator.Update(Time.deltaTime);
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

    public FrameRateCalculator()
    {
        Debug.Log("Performance Metrics: Calculating FPS.");
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
        float meanFPS = 1f / meanDeltaT;
        return meanFPS;
    }

    public string ContextualFPS()
    {
        if (times.Count < minSamples)
        {
            return "FPS: N/A";
        }

        return "FPS: " + RoundFloat(AverageFPS(), 2);
    }

    public float RoundFloat(float value, int decimals)
    {
        int scaledValue = (int)(value * Mathf.Pow(10, decimals));
        float fpsRounded = (float)scaledValue / Mathf.Pow(10, decimals);
        return fpsRounded;
    }
}