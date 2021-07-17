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
    }
}
