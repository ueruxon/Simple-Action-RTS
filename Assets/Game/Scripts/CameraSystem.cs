using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    private float _cameraY;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineTransposer _cinemachineTransposer;

    private void Awake() {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineTransposer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        _cameraY = _cinemachineTransposer.m_FollowOffset.y;
    }

    private void Update() {
        float zoomSpeed = 1f;
        _cameraY += Input.mouseScrollDelta.y * zoomSpeed;
        _cinemachineTransposer.m_FollowOffset.y = _cameraY;
    }
}
