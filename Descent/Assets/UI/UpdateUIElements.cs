using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UIInfoManager : MonoBehaviour
{
    private Dictionary<string, TextMeshProUGUI> labels = new();
    private Dictionary<string, Button> buttons = new();

    private void Awake()
    {
        labels.Add("SpeciesEnglish", GetLabel("EnglishNameLabel"));
        labels.Add("SpeciesLatin", GetLabel("ScientificNameLabel"));
        Debug.Log("Labels: " + labels.Count);
    }

    private TextMeshProUGUI GetLabel(string name)
    {
        var obj = GameObject.Find(name);
        return obj.GetComponent<TextMeshProUGUI>();
    }

    public void SetLabelText(string labelName, string value)
    {
        TextMeshProUGUI label;
        if (labels.TryGetValue(labelName, out label))
        {
            label.text = value;
            return;
        }
        Debug.Log("Set() - Couldn't find a label with key: " + labelName);
    }

    public void SetLabelVisible(string labelName, bool visible)
    {
        TextMeshProUGUI label;
        if (labels.TryGetValue(labelName, out label))
        {
            label.enabled = visible;
            return;
        }
        Debug.Log("Set() - Couldn't find a label with key: " + labelName);
    }
}