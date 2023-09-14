using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform canvas;
    private Transform chatPanel;

    public GameObject chapterPanel;
    public GameObject missionPanel;
    public GameObject chatPanelPrefab;
    public GameObject textPanelPrefab;
    public GameObject selectButtonPrefab;
    public GameObject textButtonPrefab;
    public GameObject blessingCountPanel;

    public PlayerInfo playerInfo;
    public TextScriptableObject textMessages;
    public TextScriptableObject buttonTexts;

    private void Start()
    {
        InstantiateBasePanels();
    }

    private void Awake()
    {
        StartCoroutine(CreateChat(0));
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    CreateTextButton(3);
        //}
    }

    #region Instantiate Object
    private void InstantiateChatPanel()
    {
        GameObject g = Instantiate(chatPanelPrefab, canvas);
        chatPanel = g.transform;
    }

    private void InstantiateBasePanels()
    {
        GameObject g = Instantiate(chapterPanel, canvas);
        SetChapter(g);

        g = Instantiate(missionPanel, canvas);
        SetMission(g);

        InstantiateChatPanel();
    }

    private void InstantiateText(GameObject prefab, int textNumber)
    {
        prefab.GetComponent<TextManager>().SetPanel(textNumber);
        GameObject g = Instantiate(prefab, chatPanel);
    }

    private void InstantiateButton(GameObject prefab, Transform parent)
    {
        GameObject g = Instantiate(prefab, parent);
        g.GetComponent<Button>().onClick.AddListener(() =>
        {
            SelectClick();
            Destroy(g);
        });
    }
    #endregion

    #region Sequence_1
    private void SetChapter(GameObject g)
    {
        g.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = playerInfo.chapterProgress.chapterName;
        g.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = playerInfo.chapterProgress.missionType.ToString();
    }

    private void SetMission(GameObject g)
    {
        g.transform.GetChild(0).GetComponent<TMP_Text>().text = playerInfo.chapterProgress.missionType.ToString();
        g.transform.GetChild(1).GetComponent<TMP_Text>().text = playerInfo.chapterProgress.missionName;
    }

    private IEnumerator CreateChat(int textNumber)
    {
        yield return new WaitForSeconds(1f);
        InstantiateText(textPanelPrefab, textNumber);

        if (textNumber < textMessages.text.Count - 1)
        {
            StartCoroutine(CreateChat(textNumber + 1));
        }
        else
        {
            yield return new WaitForSeconds(1f);
            InstantiateButton(selectButtonPrefab, canvas);
        }
    }
    #endregion

    #region Sequence_2
    private IEnumerator CreateTextButton(int textsCount)
    {
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < textsCount; i++)
        {
            InstantiateText(textButtonPrefab, i);
        }
    }

    private void SelectClick()
    {
        Destroy(chatPanel.gameObject);
        InstantiateChatPanel();
        SetBlessingCount();
        StartCoroutine(CreateTextButton(buttonTexts.text.Count));
    }

    private void SetBlessingCount()
    {
        blessingCountPanel.SetActive(true);
        blessingCountPanel.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = playerInfo.blessing.ToString();
    }
    #endregion
}
