using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    [SerializeField] private bool _invert;
    [SerializeField] private bool _zeroY;

    private Transform _mainCameraTransform;

    private void Awake() {
        _mainCameraTransform = Camera.main.transform;
    }

    private void Update() {
        LookAt();
    }

    private void OnEnable() {
        LookAt();
    }

    private void LookAt() {
        if (_invert) {
            Vector3 dir = (transform.position - _mainCameraTransform.position).normalized;
            transform.LookAt(transform.position + dir);
        }
        else {
            transform.LookAt(_mainCameraTransform.position);
        }

        if (_zeroY) {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
        }
    }

    public void SetInvert(bool invert) {
        _invert = invert;
    }

    public void SetZeroY(bool zeroY) {
        _zeroY = zeroY;
    }

}
