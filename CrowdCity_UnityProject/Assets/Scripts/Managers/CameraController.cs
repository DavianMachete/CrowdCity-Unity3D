using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform cam;
    [SerializeField] float elasticity = 0.5f;
    Vector3 velocity = Vector3.zero;
    Transform targetTransform;
    bool follow = false;

    Vector3 cameraForwardVector = Vector3.zero;
    Vector3 cameraUpwardVector = Vector3.zero;

    void Awake()
    {
        instance = this;
    }

    public void ChangeCameraForwardDistance(float value)
    {
        cameraForwardVector = -1f * value * camTransform.forward;
        UpdateCameraTransformPOsition();
    }

    public void ChangeCameraUpwordDistance(float value)
    {
        cameraUpwardVector = value * camTransform.up;
        UpdateCameraTransformPOsition();
    }

    private Vector3 angles;
    public void RotateCameraByXAxis(float value)
    {
        angles = Vector3.zero;
        angles.x = value;

        cam.eulerAngles = angles;
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

    private void UpdateCameraTransformPOsition()
    {
        camTransform.localPosition = cameraForwardVector + cameraUpwardVector;
    }
}
