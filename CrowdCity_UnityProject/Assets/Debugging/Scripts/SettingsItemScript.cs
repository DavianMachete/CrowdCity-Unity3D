using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// here we create our own serializable CustomEvents by inheriting from UnityEvent<T>
[System.Serializable]
public class CustomFloatEvent : UnityEvent<float> { }
[System.Serializable]
public class CustomIntEvent : UnityEvent<int> { }
[System.Serializable]
public class CustomBoolEvent : UnityEvent<bool> { }
[System.Serializable]
public class CustomVoidEvent : UnityEvent { }

public class SettingsItemScript : MonoBehaviour
{
    public GameObject InstanceObject => this.gameObject;
    [System.Serializable]
    public struct SettingDropdownItem
    {
        public string name;
        public int value;
    }

    public DebugSettingsItemTypes debugSettingsItemType;
    public string title;
    public string playerPrefsName;

    public int currentValueInt;
    public bool currentValueBool;
    public float currentValueFloatSlider;

    public float floatSliderMinValue;
    public float floatSliderMaxValue;

    public List<SettingDropdownItem> settingDropdowns;

    public CustomFloatEvent floatValueChangeCallback;
    public CustomIntEvent dropdownValueChangeCallback;
    public CustomBoolEvent toggleValueChangeCallback;

    public bool RestartOnValueChangeFromUI;
    public CustomVoidEvent restartFunction;

    private DebugSettingsItemUI connectedDebugSettingsItemUI;

    public void Initialize(DebugSettingsItemUI debugSettingsItem)
    {
        if(connectedDebugSettingsItemUI != null)
        {
            Destroy(connectedDebugSettingsItemUI.gameObject);
        }
        LoadValues();
        connectedDebugSettingsItemUI = debugSettingsItem;
        connectedDebugSettingsItemUI.Initialize(this);
    }

    void LoadValues()
    {
        if(debugSettingsItemType == DebugSettingsItemTypes.Toggle)
        {
            currentValueBool = LoadInteger(playerPrefsName, currentValueBool ? 1 : 0) == 1;
        }
        else if (debugSettingsItemType == DebugSettingsItemTypes.FloatSlider)
        {
            currentValueFloatSlider = LoadFloat(playerPrefsName, currentValueFloatSlider);
        }
        else if(debugSettingsItemType == DebugSettingsItemTypes.Dropdown)
        {
            currentValueInt = LoadInteger(playerPrefsName, currentValueInt);
        }
    }

    public void OnBoolValueChangeFromUI(bool newValue)
    {
        PlayerPrefs.SetInt(playerPrefsName, newValue ? 1 : 0);

        currentValueBool = newValue;

        toggleValueChangeCallback?.Invoke(currentValueBool);

        OnValueChangeFromUI();
    }

    public void OnFloatValueChangeFromUI(float newValue)
    {
        PlayerPrefs.SetFloat(playerPrefsName, newValue);
        currentValueFloatSlider = newValue;

        floatValueChangeCallback?.Invoke(currentValueFloatSlider);

        OnValueChangeFromUI();
    }

    public void OnDropdownValueChangeFromUI(int newValue)
    {
        PlayerPrefs.SetInt(playerPrefsName, newValue);

        currentValueInt = newValue;

        dropdownValueChangeCallback?.Invoke(settingDropdowns[newValue].value);

        OnValueChangeFromUI();
    }
    public int GetDropdownValue()
    {
        return settingDropdowns[currentValueInt].value;
    }

    public virtual void OnValueChangeFromUI() {
        // this function can invoke some events or call function every time smth from debug menu is changed from UI
        restartFunction?.Invoke();
    }

    public void UpdateUIValue(bool newValue)
    {
        connectedDebugSettingsItemUI.SetBool(newValue);
    }
    public void UpdateUIValue(float newValue)
    {
        connectedDebugSettingsItemUI.SetFloat(newValue);
    }

    public void UpdateDropdownUIValue(int value)
    {
        connectedDebugSettingsItemUI.SetDropdownValue(value);
    }


    private int LoadInteger(string Name, int defaultValue)
    {
        if(PlayerPrefs.HasKey(Name))
        {
            return PlayerPrefs.GetInt(Name);
        }
        else
        {
            PlayerPrefs.SetInt(Name, defaultValue);

            return defaultValue;
        }

    }

    private float LoadFloat(string Name, float defaultValue)
    {
        if (PlayerPrefs.HasKey(Name))
        {
            return PlayerPrefs.GetFloat(Name);
        }
        else
        {
            PlayerPrefs.SetFloat(Name, defaultValue);

            return defaultValue;
        }
    }
}

public enum DebugSettingsItemTypes { Toggle, FloatSlider, Dropdown };
