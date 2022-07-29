using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FingerController : MonoBehaviour
{
    [SerializeField] List<Sprite> fingerStates = new List<Sprite>();
    [SerializeField] Image finger;

    public void Update()
    {
        SetFingerPosition();
    }

    public void SetFingerPosition()
    {
        transform.position = Input.mousePosition;
    }

    public void Push()
    {
        if (PushRoutineC != null) StopCoroutine(PushRoutineC);
        PushRoutineC = StartCoroutine(PushRoutine());
    }

    public void Release()
    {
        if (ReleaseRoutineC != null) StopCoroutine(ReleaseRoutineC);
        ReleaseRoutineC = StartCoroutine(ReleaseRoutine());
    }

    public void Click()
    {
        if (ClickRoutineC != null) StopCoroutine(ClickRoutineC);
        ClickRoutineC = StartCoroutine(ClickRoutine());
    }

    Coroutine PushRoutineC;
    IEnumerator PushRoutine() {
        finger.transform.DOScale(0.9f, 0.25f).SetEase(Ease.OutSine);

        finger.sprite = fingerStates[2];
        yield return new WaitForSeconds(0.02f);
        finger.sprite = fingerStates[1];
        yield return new WaitForSeconds(0.02f);
        finger.sprite = fingerStates[0];
    }

    Coroutine ReleaseRoutineC;
    IEnumerator ReleaseRoutine()
    {
        finger.transform.DOScale(1f, 0.25f).SetEase(Ease.OutSine);

        finger.sprite = fingerStates[0];
        yield return new WaitForSeconds(0.02f);
        finger.sprite = fingerStates[1];
        yield return new WaitForSeconds(0.02f);
        finger.sprite = fingerStates[2];
    }


    Coroutine ClickRoutineC;
    IEnumerator ClickRoutine()
    {
        finger.sprite = fingerStates[2];
        yield return new WaitForSeconds(0.02f);
        finger.sprite = fingerStates[1];
        yield return new WaitForSeconds(0.02f);
        finger.sprite = fingerStates[0];
        yield return new WaitForSeconds(0.02f);
        finger.sprite = fingerStates[1];
        yield return new WaitForSeconds(0.02f);
        finger.sprite = fingerStates[2];
    }
}
