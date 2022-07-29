using System;
using UnityEngine;

[Serializable]
public class OnTagCollisionEnter
{
    public string tag;
    public OnCollisionEnter onCollisionEnter;

    public void CheckAndInvoke(Collision collision)
    {
        if (collision.gameObject.tag == tag)
            onCollisionEnter?.Invoke(collision);
    }
}
