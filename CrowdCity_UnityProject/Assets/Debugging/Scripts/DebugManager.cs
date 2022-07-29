using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Linq;

[System.Serializable]
public class DebugManager : MonoBehaviour
{
    public TextMeshProUGUI frameCounter;
    public DebugSettingsItemUI debugSettingsItemUIReference;

    [HideInInspector] public List<SettingsItemScript> settingsItemScripts ;
    public static DebugManager instance;
    public void FindAllSettingsItems()
    {
        IEnumerable<SettingsItemScript> finded_ones = FindObjectsOfType<SettingsItemScript>();
        if (settingsItemScripts != null)
        {
            settingsItemScripts.RemoveAll((SettingsItemScript a) => a == null);
            settingsItemScripts = settingsItemScripts.Union(finded_ones).ToList();
        }
        else
        {
            settingsItemScripts = finded_ones.ToList();
        }
    }

    public void Awake()
    {
        instance = this;
        //Init();
    }
    public void Init()
    {
        if(settingsItemScripts == null)
        {
            FindAllSettingsItems();
        }
        foreach (var sis in settingsItemScripts)
        {
            var debugSettingsItemUI = GetDebugSettingsItem();
            sis.Initialize(debugSettingsItemUI);
        }
    }

    private void Start()
    {
        StartMeasuringFPS();
    }

    private DebugSettingsItemUI GetDebugSettingsItem()
    {
        return Instantiate(debugSettingsItemUIReference.gameObject, debugSettingsItemUIReference.transform.parent).GetComponent<DebugSettingsItemUI>();
    }

    private void StartMeasuringFPS()
    {
        if (fpsMeasuringCoroutine != null) StopCoroutine(fpsMeasuringCoroutine);
        fpsMeasuringCoroutine = StartCoroutine(FPSMeasuringRoutine());
    }

    private Coroutine fpsMeasuringCoroutine;
    private IEnumerator FPSMeasuringRoutine()
    {
        int m_frameCounter = 0;
        float m_timeCounter = 0.0f;
        float m_lastFramerate = 0.0f;
        float m_refreshTime = 0.5f;

        while (true)
        {
            if (m_timeCounter < m_refreshTime)
            {
                m_timeCounter += Time.deltaTime;
                m_frameCounter++;
            }
            else
            {
                //This code will break if you set your m_refreshTime to 0, which makes no sense.
                m_lastFramerate = m_frameCounter / m_timeCounter;
                m_frameCounter = 0;
                m_timeCounter = 0.0f;

                frameCounter.text = $"FPS {(int)m_lastFramerate}";
            }
            yield return null; //wait for a single frame
        }
    }

    //private void insertionSort(SettingsItemScript[] arr)
    //{
    //    int n = arr.Length;
    //    for (int i = 1; i < n; ++i)
    //    {
    //        SettingsItemScript key = arr[i];
    //        int j = i - 1;

    //        while (j >= 0 && arr[j].GetInstanceID() > key.GetInstanceID())
    //        {
    //            arr[j + 1] = arr[j];
    //            j = j - 1;
    //        }
    //        arr[j + 1] = key;
    //    }
    //}
}

