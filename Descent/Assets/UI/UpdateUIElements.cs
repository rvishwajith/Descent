using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInfoManager : MonoBehaviour
{
    private Dictionary<string, TextMeshProUGUI> labels = new();

    private void Awake()
    {
        labels.Add("SpeciesEnglish", GetLabel("EnglishNameLabel"));
        labels.Add("SpeciesLatin", GetLabel("ScientificNameLabel"));
    }

    private TextMeshProUGUI GetLabel(string name)
    {
        var obj = GameObject.Find(name);
        return obj.GetComponent<TextMeshProUGUI>();
    }

    public void SetLabelText(string labelKey, string value)
    {
        TextMeshProUGUI label;
        if (labels.TryGetValue(labelKey, out label))
        {
            label.text = value;
            return;
        }
        Debug.Log("Set() - Couldn't find a label with key: " + labelKey);
    }
}