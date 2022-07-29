using System;
using UnityEngine;

[Serializable]
public class OnTagTriggerEnter 
{
    public string tag;
    public OnTriggerEnter onTriggerEnter;

    public void CheckAndInvoke(Collider collider)
    {
        if (collider.tag == tag)
            onTriggerEnter?.Invoke(collider);
    }
}
