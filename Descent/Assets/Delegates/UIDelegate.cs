using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDelegate : MonoBehaviour
{
    private void Start() { }

    private void Update()
    {

    }

    private void Animate(Transform transform, string property, float duration = 0, float delay = 0, int easing = 1, bool repeating = false)
    {

    }
}

struct UIDelegateNode
{
    public Transform transform;
    public Image image;
}