using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPanelManager : TextManager
{
    private float[] textRectHeight = new float[3] { 60, 120, 20 };

    //private void OnValidate()
    //{
    //    if (Application.isPlaying)
    //    {
    //        SetTextMessage(textNumber);
    //        SetPanel(textNumber);
    //    }
    //}

    public override void SetPanel(int textNumber)
    {
        this.textNumber = textNumber;
        panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, 70 + textRectHeight[textNumber]);

        SetTextMessage(textNumber);
    }

    public override void SetTextMessage(int textNumber)
    {
        text.text = textMessage.text[textNumber];
    }
}
