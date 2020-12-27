using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    public Vector3 startingRotation;
    public float clampAngle = 90f;
    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;

    private InputManager inputManager;

    protected override void Awake()
    {
        inputManager = InputManager.Instance;
        startingRotation = transform.localRotation.eulerAngles;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = inputManager.GetMouseDelta();
                startingRotation.x += deltaInput.x * verticalSpeed * Time.unscaledDeltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.unscaledDeltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
    }
}
