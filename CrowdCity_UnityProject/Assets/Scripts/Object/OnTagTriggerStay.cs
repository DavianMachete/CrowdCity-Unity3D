using System;
using UnityEngine;

[Serializable]
public class OnTagTriggerStay
{
    public string tag;
    public OnTriggerStay onTriggerStay;

    public void CheckAndInvoke(Collider collider)
    {
        if (collider.CompareTag(tag))
            onTriggerStay?.Invoke(collider);
    }
}
