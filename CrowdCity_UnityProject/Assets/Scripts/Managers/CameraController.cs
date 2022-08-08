using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform cam;
    [SerializeField] float elasticity = 0.5f;
    [SerializeField] float cameraDistanceStep = 2f;
    [SerializeField] float cameraUpdateDistanceCount = 50f;
    [SerializeField] float cameraUpdateDistanceElasticity = 20f;

    Vector3 velocity = Vector3.zero;
    Transform targetTransform;
    bool follow = false;

    Vector3 cameraForwardVector = Vector3.zero;
    Vector3 cameraUpwardVector = Vector3.zero;

    float cameraDistance = 0f;

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
        StopUpdateCameraDistance();
        cameraDistance = 0f;
        
        SetTargetToFollow(PlayerController.instance.characterController.transform);

        Crowd playerCrowd = CrowdManager.instance.GetCrowd(Clan.Player);

        playerCrowd.OnCountUpdated += UpdateCameraDistance;

        follow = true;
    }

    private void LateUpdate()
    {
        if (!follow)
            return;

        transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * elasticity);
    }

    private void UpdateCameraDistance(int count)
    {
        cameraDistance = Mathf.FloorToInt((float)count / cameraUpdateDistanceCount) * cameraDistanceStep;

        StartUpdateCameraDistance();
    }

    private void UpdateCameraTransformPOsition()
    {
        camTransform.localPosition = cameraForwardVector + cameraUpwardVector +
            (-1f * cameraDistance * camTransform.forward);
    }

    private void StartUpdateCameraDistance()
    {
        if (IUpdateCameraDistanceHelper == null)
            IUpdateCameraDistanceHelper = StartCoroutine(IUpdateCameraDistance());
    }

    private void StopUpdateCameraDistance()
    {
        if (IUpdateCameraDistanceHelper != null)
            StopCoroutine(IUpdateCameraDistanceHelper);
    }

    private Coroutine IUpdateCameraDistanceHelper;
    private IEnumerator IUpdateCameraDistance()
    {
        Vector3 targetPos = cameraForwardVector + cameraUpwardVector +
            (-1f * cameraDistance * camTransform.forward);

        while (Vector3.Distance(camTransform.localPosition, targetPos) > 0.01f)
        {
            targetPos = cameraForwardVector + cameraUpwardVector +
            (-1f * cameraDistance * camTransform.forward);

            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, targetPos, Time.deltaTime * cameraUpdateDistanceElasticity);

            yield return new WaitForEndOfFrame();
        }

        IUpdateCameraDistanceHelper = null;
    }
}
