using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class DebugCanvasController : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] TextMeshProUGUI openText;
    [SerializeField] TextMeshProUGUI closeText;

    [Tooltip("This are the scroll view tabs that you want to be able to switch")]
    [SerializeField] List<GameObject> settingsViews;

    void Start()
    {
        HidePanel();
        OpenView(0);

        //EditorApplication.playModeStateChanged += OnPlayModeStateChange;
    }

    public void PanelToggleClicked()
    {
        // toggle close/open text
        if (settingsPanel.activeSelf)
        {
            HidePanel();
        }
        else
        {
            OpenPanel();
        }
    }

    void OpenPanel()
    {
        settingsPanel.SetActive(true);

        // show "close" text
        openText.gameObject.SetActive(false);
        closeText.gameObject.SetActive(true);
    }

    void HidePanel()
    {
        settingsPanel.SetActive(false);

        // show "open" text
        openText.gameObject.SetActive(true);
        closeText.gameObject.SetActive(false);
    }

    public void TransparentDebugMenuOpenCloseButton(bool transparent)
    {
        if (transparent)
        {
            openText.color = new Color(openText.color.r, openText.color.b, openText.color.b, 0f);
            closeText.color = new Color(closeText.color.r, closeText.color.b, closeText.color.b, 0f);
        }

        else
        {
            openText.color = new Color(openText.color.r, openText.color.b, openText.color.b, 1f);
            closeText.color = new Color(closeText.color.r, closeText.color.b, closeText.color.b, 1f);
        }

    }

    public void OpenView(int pageIndex)
    {
        for (int i = 0; i < settingsViews.Count; i++)
        {
            settingsViews[i].SetActive(i == pageIndex);
        }
    }

    //void OnPlayModeStateChange(PlayModeStateChange state)
    //{
    //    if (state == PlayModeStateChange.EnteredEditMode)
    //    {
    //        settingsPanel.SetActive(false);
    //    }
    //}
}
