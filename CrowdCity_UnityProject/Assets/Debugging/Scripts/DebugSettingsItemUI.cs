using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugSettingsItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleTxt;
    [SerializeField] GameObject ToggleContainer;
    [SerializeField] GameObject DropdownContainer;
    [SerializeField] GameObject FloatSliderContainer;


    private SettingsItemScript connectedSettingsItemScript;
    private bool sliderIgnorePropagation = true;

    private float currentValueFloat;
    private bool currentValueBool;
    private int currentDropdownValue => dropdown.value;

    public void Initialize(SettingsItemScript debugSettingsItem_)
    {
        gameObject.SetActive(true);
        connectedSettingsItemScript = debugSettingsItem_;

        ToggleContainer.SetActive(connectedSettingsItemScript.debugSettingsItemType == DebugSettingsItemTypes.Toggle);
        DropdownContainer.SetActive(connectedSettingsItemScript.debugSettingsItemType == DebugSettingsItemTypes.Dropdown);
        FloatSliderContainer.SetActive(connectedSettingsItemScript.debugSettingsItemType == DebugSettingsItemTypes.FloatSlider);


        if (connectedSettingsItemScript.debugSettingsItemType == DebugSettingsItemTypes.FloatSlider)
        {
            sliderMinValueText.text = connectedSettingsItemScript.floatSliderMinValue.ToString();
            sliderMaxValueText.text = connectedSettingsItemScript.floatSliderMaxValue.ToString();

            slider.minValue = connectedSettingsItemScript.floatSliderMinValue;
            slider.maxValue = connectedSettingsItemScript.floatSliderMaxValue;

            SetFloat(connectedSettingsItemScript.currentValueFloatSlider);
            connectedSettingsItemScript.floatValueChangeCallback?.Invoke(currentValueFloat);

        }
        else if (connectedSettingsItemScript.debugSettingsItemType == DebugSettingsItemTypes.Dropdown)
        {
            InitDropdownValues(connectedSettingsItemScript.settingDropdowns);
            SetDropdownValue(connectedSettingsItemScript.currentValueInt);
            connectedSettingsItemScript.dropdownValueChangeCallback?.Invoke(currentDropdownValue);
        }

        else if (connectedSettingsItemScript.debugSettingsItemType == DebugSettingsItemTypes.Toggle)
        {
            SetBool(connectedSettingsItemScript.currentValueBool);
            connectedSettingsItemScript.toggleValueChangeCallback?.Invoke(currentValueBool);
        }


        titleTxt.text = connectedSettingsItemScript.title;
    }

    //Toggle
    [SerializeField] RectTransform toggleKnob;
    [SerializeField] Image toggleBackground;
    [SerializeField] Color toggleOnBg;
    [SerializeField] Color toggleoffBg;

    public void SetBool(bool currentValueBool_)
    {
        currentValueBool = currentValueBool_;
        UpdateToggleUI(currentValueBool);
    }


    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI sliderMinValueText;
    [SerializeField] TextMeshProUGUI sliderMaxValueText;
    [SerializeField] TextMeshProUGUI sliderCurrentValueText;
    public void SetFloat(float currentValueFloat_)
    {
        currentValueFloat = currentValueFloat_;
        UpdateSliderUI(currentValueFloat);
    }

    public void SetDropdownValue(int currentValueInt)
    {
        UpdateDropdownUI(currentValueInt);
    }
    //Dropdown
    [SerializeField] TMP_Dropdown dropdown;
    [HideInInspector] public List<SettingsItemScript.SettingDropdownItem> dropdownItems;

    public void InitDropdownValues(List<SettingsItemScript.SettingDropdownItem> dropdownItems_)
    {
        dropdownItems = dropdownItems_;

        dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < dropdownItems.Count; i++)
        {
            TMP_Dropdown.OptionData newData = new TMP_Dropdown.OptionData();

            newData.text = dropdownItems[i].name;

            dropdownOptions.Add(newData);

        }

        dropdown.AddOptions(dropdownOptions);
    }

    public void VisibleDelay()
    {
        if (DropdownBecomeVisibleC != null) StopCoroutine(DropdownBecomeVisibleC);
        DropdownBecomeVisibleC = StartCoroutine(DropdownBecomeVisible());
    }


    Coroutine DropdownBecomeVisibleC;
    IEnumerator DropdownBecomeVisible()
    {
        Transform listTransform = dropdown.transform.Find("Dropdown List");
        
        while(listTransform == null)
        {
            listTransform = dropdown.transform.Find("Dropdown List");
            yield return new WaitForFixedUpdate();
        }

        listTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(listTransform.GetComponent<RectTransform>().sizeDelta.x, 400);
        
    }

    public void OnDropdownValueChange()
    {
        connectedSettingsItemScript.OnDropdownValueChangeFromUI(currentDropdownValue);
    }


    //UI Events
    public void ToggleKnobValueClicked()
    {
        currentValueBool = !currentValueBool;
        UpdateToggleUI(currentValueBool);
        connectedSettingsItemScript.OnBoolValueChangeFromUI(currentValueBool);
    }

    public void SliderValueChanged()
    {
        if (sliderIgnorePropagation) return;
        currentValueFloat = slider.value;
        sliderCurrentValueText.text = currentValueFloat.ToString();

        connectedSettingsItemScript.OnFloatValueChangeFromUI(currentValueFloat);
    }


    //Update UI
    void UpdateToggleUI(bool value)
    {
        toggleBackground.color = value ? toggleOnBg : toggleoffBg;
        toggleKnob.anchoredPosition = new Vector2(value ? toggleKnob.parent.GetComponent<RectTransform>().rect.width / 2 : 0, 0);
    }

    void UpdateSliderUI(float value)
    {
        sliderIgnorePropagation = true;
        slider.value = value;
        sliderCurrentValueText.text = value.ToString();

        Invoke("SliderEnableListener",0.1f);
    }

    void UpdateDropdownUI(int value)
    {
        dropdown.SetValueWithoutNotify(value);
    }

    void SliderEnableListener()
    {
        sliderIgnorePropagation = false;
    }
}
