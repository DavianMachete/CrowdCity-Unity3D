using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrowdCounterController : MonoBehaviour
{
    [SerializeField] private TMP_Text countTmp;
    [SerializeField] private Image counterBgImage;
    [SerializeField] private Image counterArrowImage;
    [SerializeField] private RectTransform counter;
    [SerializeField] private RectTransform counterBG;
    [SerializeField] private RectTransform counterArrow;

    [SerializeField] private float elasticity = 20f;

    [SerializeField] private float worldPosHeight = 3f;
    [SerializeField] private float counterWidthStep = 60f;

    [SerializeField] private float maxY;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float minX;

    [SerializeField] private int paddingTop = 300;
    [SerializeField] private int paddingBottom = 20;
    [SerializeField] private int paddingRight = 150;
    [SerializeField] private int paddingLeft = 150;

    private Transform leaderTransform;
    private Canvas canvas;
    private Crowd crowd;
    private Vector2 anchoredPosition;
    private Vector2 anchoredPositionHolder;
    private Vector3 eulers;
    private bool prepared = false;
    private bool dataIsReversed;

    public void SetCanvas(Canvas canvas)
    {
        this.canvas = canvas;
        Vector2 size = canvas.GetComponent<RectTransform>().sizeDelta;
        maxY = size.y / 2f;
        minY = maxY * -1f;
        maxX = size.x / 2f;
        minX = maxX * -1f;
    }

    public void Prepare(CharacterController leader)
    {
        gameObject.name = $"{leader.name}'s crowd counter";
        leaderTransform = leader.transform;
        counterBgImage.color = CrowdManager.instance.GetCrowdLeaderMaterial(leader.Clan).color;
        counterArrowImage.color = counterBgImage.color;
        crowd = CrowdManager.instance.GetCrowd(leader.Clan);
        UpdateCounterTMP(crowd.Count);
        crowd.OnCountUpdated += UpdateCounterTMP;

        prepared = true;
    }

    public void DestroyCounter()
    {
        crowd.OnCountUpdated -= UpdateCounterTMP;
        prepared = false;
        Destroy(gameObject);
    }

    private void UpdateCounterTMP(int count)
    {
        countTmp.text = count.ToString();
        float width = 100f;
        if (count > 9)
        {
            width += counterWidthStep;
            if (count > 99)
            {
                width += counterWidthStep;
                if (count > 999)
                {
                    width += counterWidthStep;
                }
            }
        }
        counterBG.sizeDelta = new Vector2(width, counterBG.sizeDelta.y);
    }

    private void Update()
    {
        if (!prepared)
            return;


        Vector3 leaderPos = leaderTransform.position;
        leaderPos.y += worldPosHeight;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(leaderPos);

        float h = Screen.height;
        float w = Screen.width;
        float x = screenPos.x - (w / 2);
        float y = screenPos.y - (h / 2);
        float s = canvas.scaleFactor;

        anchoredPosition.x = x / s;
        anchoredPosition.y = y / s;

        dataIsReversed = false;
        if (Vector3.Dot(Camera.main.transform.forward, (leaderPos - Camera.main.transform.position).normalized) < 0)
            dataIsReversed = true;
        if (dataIsReversed)
            anchoredPosition *= -1f;
        anchoredPositionHolder = anchoredPosition;


        anchoredPosition.x = Mathf.Clamp(anchoredPositionHolder.x, minX + paddingLeft, maxX - paddingRight);
        anchoredPosition.y = Mathf.Clamp(anchoredPositionHolder.y, minY + paddingBottom, maxY - paddingTop);


        //if (name.Contains("Opponent"))
        //    Debug.Log(name + " clamped --> " + anchoredPosition);

        Vector2 dir = anchoredPositionHolder + counterBG.anchoredPosition - anchoredPosition;
        eulers.z = Vector2.SignedAngle(dir.normalized, Vector2.up);
        if (dataIsReversed)
            //eulers.z -= 180f;//Vector2.SignedAngle(dir.normalized, Vector2.up * -1f);
        eulers.z = Mathf.Clamp(eulers.z, -90f, 90f);
        counterArrow.rotation = Quaternion.Lerp(counterArrow.rotation, Quaternion.Euler(eulers), Time.deltaTime * elasticity);

        float t = Mathf.InverseLerp(-90f, 90f, eulers.z);
        Vector2 bgSize = counterBG.sizeDelta;
        bgSize.x -= 100f;
        bgSize.y = 0f;
        counterArrow.anchoredPosition = Vector2.Lerp(counterArrow.anchoredPosition,Vector2.Lerp(bgSize / -2f, bgSize / 2f, t), Time.deltaTime * elasticity);

        counter.anchoredPosition = Vector2.Lerp(counter.anchoredPosition, anchoredPosition, Time.deltaTime * elasticity);
    }
}
