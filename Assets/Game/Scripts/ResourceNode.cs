using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourceTypeSO _resourceTypeSO;
    [SerializeField] private int resourceAmount = 5;

    public Vector3 GetPosition() {
        return transform.position;
    }

    public ResourceTypeSO GetResourceTypeSO() {
        return _resourceTypeSO;
    }

    public void GrabResource() {
        resourceAmount--;

        if (resourceAmount <= 0) {
            Destroy(gameObject);
        }
    }

    public bool HasResources() {
        return resourceAmount > 0;
    }
}
