using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUtils : MonoBehaviour
{
    public static List<GameObject> InstantiateActionPositions(List<Vector3> positions, GameObject prefab)
    {
        List<GameObject> actionGameObjects = new List<GameObject>();
        if (positions.Count > 0)
        {
            foreach (Vector3 position in positions)
            {
                actionGameObjects.Add(InstantiateActionPosition(position, prefab));
            }
        }
        return actionGameObjects;
    }

    public static GameObject InstantiateActionPosition(Vector3 position, GameObject prefab)
    {
        prefab.transform.position = new Vector3(position.x, position.y, 0.95f);
        return Instantiate(prefab);
    }

    public static void Clear(List<GameObject> actionGameObjects)
    {
        foreach (GameObject gameobject in actionGameObjects)
        {
            Destroy(gameobject);
        }
        actionGameObjects.RemoveAll(dest => true);
    }
}
