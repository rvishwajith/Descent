using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateUIElements : MonoBehaviour
{
    [Header("Labels")]
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

    public void SetLabel(string key, string value)
    {
        TextMeshProUGUI label;
        if (labels.TryGetValue(key, out label))
        {
            label.text = value;
            return;
        }
        Debug.Log("Set() - Couldn't find a label with key: " + key);
    }
}

