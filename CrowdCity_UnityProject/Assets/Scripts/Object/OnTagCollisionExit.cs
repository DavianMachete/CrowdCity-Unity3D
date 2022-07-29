using System;
using UnityEngine;

[Serializable]
public class OnTagCollisionExit
{
    public string tag;
    public OnCollisionExit onCollisionExit;

    public void CheckAndInvoke(Collision collision)
    {
        if (collision.gameObject.tag == tag)
            onCollisionExit?.Invoke(collision);
    }
}
