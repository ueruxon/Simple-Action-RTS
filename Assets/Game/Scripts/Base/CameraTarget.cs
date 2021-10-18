using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public enum Axis {
        XZ,
        XY
    }

    [SerializeField] private Axis _axis = Axis.XZ;
    [SerializeField] private float _moveSpeed = 30f;

    private float _cameraY;

    private void Update() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDir;

        switch (_axis) {
            default:
            case Axis.XZ:
                moveDir = new Vector3(moveX, 0, moveY).normalized;
                break;
            case Axis.XY:
                moveDir = new Vector3(moveX, moveY).normalized;
                break;
        }

        transform.position += moveDir * _moveSpeed * Time.deltaTime;


        float zoomSpeed = 1f;
        _cameraY += -Input.mouseScrollDelta.y * zoomSpeed;
        _cameraY = Mathf.Clamp(_cameraY, -3f, 17);
        transform.position = new Vector3(transform.position.x, _cameraY, transform.position.z);
    }
}
