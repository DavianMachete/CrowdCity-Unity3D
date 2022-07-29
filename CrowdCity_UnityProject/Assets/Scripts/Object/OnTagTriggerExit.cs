using System;
using UnityEngine;

[Serializable]
public class OnTagTriggerExit
{
    public string tag;
    public OnTriggerExit onTriggerExit;

    public void CheckAndInvoke(Collider collider)
    {
        if (collider.tag == tag)
            onTriggerExit?.Invoke(collider);
    }
}
