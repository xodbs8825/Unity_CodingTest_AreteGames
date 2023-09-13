using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public Transform chatPanel;

    public GameObject textPanelPrefab;

    public TextScriptableObject textMessages;

    private void Awake()
    {
        StartCoroutine(CreateChat(0));
    }

    private void InstantiateChat(int textNumber)
    {
        textPanelPrefab.GetComponent<TextPanelManager>().SetPanel(textNumber);

        GameObject g = Instantiate(textPanelPrefab);
        g.transform.SetParent(chatPanel, false);
    }


    private IEnumerator CreateChat(int textNumber)
    {
        yield return new WaitForSeconds(1f);

        InstantiateChat(textNumber);

        if (textNumber < textMessages.text.Count - 1)
        {
            StartCoroutine(CreateChat(textNumber + 1));
        }
    }
}
