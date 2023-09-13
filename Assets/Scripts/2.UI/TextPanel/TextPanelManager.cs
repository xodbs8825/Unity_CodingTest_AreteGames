using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPanelManager : MonoBehaviour
{
    public RectTransform panelRectTransform;
    public TMP_Text text;

    public TextScriptableObject textMessage;
    public int textNumber;

    private float[] textRectHeight = new float[3] { 60, 120, 20 };
    private float titleRectHeight = 70;

    //private void OnValidate()
    //{
    //    if (Application.isPlaying)
    //    {
    //        SetTextMessage(textNumber);
    //        SetPanel(textNumber);
    //    }
    //}

    public void SetPanel(int textNumber)
    {
        float x = (float)panelRectTransform.rect.x;
        float y = (float)panelRectTransform.rect.y;
        this.textNumber = textNumber;

        panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, titleRectHeight + textRectHeight[textNumber]);

        SetTextMessage(textNumber);
    }

    private void SetTextMessage(int textNumber)
    {
        text.text = textMessage.text[textNumber];
    }
}
