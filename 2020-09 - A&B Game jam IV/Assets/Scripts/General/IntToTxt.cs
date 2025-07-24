using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NDream;

public class IntToTxt : MonoBehaviour
{
    
    public string preText = "";
    public string posText = "";

    public IntReference var;

    
    private Text text;
    private int lastValue;

    private void Start()
    {
        text = GetComponent<Text>();

        lastValue = var.Value;
        text.text = preText + var.Value + posText;
    }

    private void Update()
    {
        UpdateHUD();   
    }

    private void UpdateHUD()
    {
        if (lastValue == var.Value)
            return;
        
        lastValue = var.Value;
        text.text = preText + var.Value + posText;
    }
}