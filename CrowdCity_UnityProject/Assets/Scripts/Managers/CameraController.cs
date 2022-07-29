using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] float elasticity = 0.5f;
    Vector3 velocity = Vector3.zero;
    Transform targetTransform;
    bool follow = false;

    void Awake()
    {
        instance = this;
    }

    public void SetTargetToFollow(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
    }

    public void InitCamera()
    {
        SetTargetToFollow(PlayerController.instance.characterController.transform);

        follow = true;
    }

    private void LateUpdate()
    {
        if (!follow)
            return;

        transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * elasticity);
    }
}
