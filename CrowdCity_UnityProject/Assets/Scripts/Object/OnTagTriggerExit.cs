using System;
using UnityEngine;

[Serializable]
public class OnTagTriggerExit
{
    public string tag;
    public OnTriggerExit onTriggerExit;

    public void CheckAndInvoke(Collider collider)
    {
        if (collider.CompareTag(tag))
            onTriggerExit?.Invoke(collider);
    }
}
