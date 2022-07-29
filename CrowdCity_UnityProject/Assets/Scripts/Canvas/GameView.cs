using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameView : MonoBehaviour
{
    [SerializeField] GameObject blackPanelsContainer;
    //[SerializeField] GameObject floatingJoystick;

    [SerializeField] RectTransform topHalf;
    [SerializeField] RectTransform bottomHalf;

    [SerializeField] RectTransform reference;
    [SerializeField] bool areBlackPanelsActive;

#if UNITY_EDITOR
    void Update()
    {
        ResizeWhiteBars();

        if (areBlackPanelsActive) { blackPanelsContainer.gameObject.SetActive(true); }
        else { blackPanelsContainer.gameObject.SetActive(false); }
    }
#endif

    void ResizeWhiteBars()
    {
        float size = Mathf.Abs(reference.sizeDelta.y / 2);
        topHalf.sizeDelta = new Vector2(topHalf.sizeDelta.x, size);
        bottomHalf.sizeDelta = new Vector2(bottomHalf.sizeDelta.x, size);
    }

}
