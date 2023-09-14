using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public RectTransform panelRectTransform;
    public RectTransform titleRectTransform;

    public TMP_Text title;
    public TMP_Text text;

    public TextScriptableObject textMessage;
    public int textNumber;

    public virtual void SetPanel(int textNumber) { }
    public virtual void SetTextMessage(int textNumber) { }
}
