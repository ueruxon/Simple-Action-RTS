using UnityEngine;

public class ResourceNodeVisualInstantiate : MonoBehaviour
{
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Transform[] visualTransformArray;

    private void Awake() {
        Destroy(visualTransform.gameObject);

        Transform newVisualTransform = Instantiate(visualTransformArray[Random.Range(0, visualTransformArray.Length)], transform);
        newVisualTransform.localPosition = Vector3.zero;
        newVisualTransform.localEulerAngles = new Vector3(0, Random.Range(0, 360f), 0);
    }
}
