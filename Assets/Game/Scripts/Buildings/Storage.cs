using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private static List<Storage> instanceList;

    public static Storage GetClosestStorage(Vector3 position) {
        if (instanceList == null) return null;

        Storage closest = null;
        foreach (Storage storage in instanceList) {
            bool isBuilt = storage.GetComponent<BuildingConstruction>().IsBuilt;

            if (isBuilt) {
                if (closest == null) {
                    closest = storage;
                }
                else {
                    if (Vector3.Distance(position, storage.GetPosition()) < Vector3.Distance(position, closest.GetPosition())) {
                        closest = storage;
                    }
                }
            }
        }
        return closest;
    }


    private void Awake() {
        if (instanceList == null) {
            instanceList = new List<Storage>();
        }

        instanceList.Add(this);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }
}
