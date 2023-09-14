using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextButtonManager : TextManager
{
    public GameObject panelSelected;
    public Button confirmButton;
    public Outline outline;

    private float[] textRectHeight = new float[3] { 90, 60, 30 };
    private bool isSelected;

    public override void SetPanel(int textNumber)
    {
        this.textNumber = textNumber;
        panelRectTransform.sizeDelta = new Vector2(panelRectTransform.sizeDelta.x, titleRectTransform.sizeDelta.y + textRectHeight[textNumber] + 60f);

        SetTextMessage(textNumber);
    }

    public override void SetTextMessage(int textNumber)
    {
        title.text = textMessage.title[textNumber];
        text.text = textMessage.text[textNumber];
    }

    public void OnPointerEnter()
    {
        if (!isSelected)
        {
            outline.gameObject.SetActive(true);
            outline.effectColor = Color.white;
        }
        else
        {
            outline.effectColor = new Color(222f, 204f, 143f, 255f);
        }
    }

    public void OnPointerExit()
    {
        if (!isSelected)
        {
            outline.gameObject.SetActive(false);
        }
    }

    public void OnSelect()
    {
        isSelected = true;
        outline.effectColor = new Color(222f, 204f, 143f, 255f);
        outline.gameObject.SetActive(true);
        panelSelected.SetActive(true);
    }

    public void OnDeselect()
    {
        isSelected = false;
        outline.gameObject.SetActive(false);
        panelSelected.SetActive(false);
    }

    public void OnClickConfirm()
    {
        Destroy(transform.parent.gameObject);
    }
}
