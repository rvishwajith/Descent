using System;
using UnityEngine;

[Serializable]
public class BakeableShaderCurve
{
    [SerializeField] private AnimationCurve curve;

    [Header("Baking Options")]
    [SerializeField] private Texture2D outputTexture;
    [SerializeField] private Vector2Int resolution = new(512, 1);
    [SerializeField] private TextureFormat format = TextureFormat.RFloat;
    [SerializeField] private bool clickToBake = false;

    public void Update()
    {
        if (clickToBake && outputTexture != null && Application.isEditor)
        {
            outputTexture.Reinitialize(resolution.x, 1, format, false);
            outputTexture.SetPixels(0, 0, resolution.x, 1, GetColors());
            outputTexture.Apply(true, false);
            Debug.Log("Finished baking!");
        }
        clickToBake = false;
    }

    public Color[] GetColors()
    {
        int i = 0;
        Color[] output = new Color[resolution.x];

        while (i < output.Length)
        {
            var t = i * 1f / output.Length;
            output[i] = new(curve.Evaluate(t), curve.Evaluate(t), curve.Evaluate(t));
            i++;
        }
        return output;
    }
}