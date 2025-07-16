using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum PoiPrefabName
{
    unset,
    TuberField
}

[CreateAssetMenu(fileName = "New Poi Data", menuName = "Data Objects/Poi Data")]
public class PoiData : ScriptableObject
{
    //Declarations
    [SerializeField] private List<GameObject> _poiPrefabs = new();







    //Externals
    public GameObject GetPoiPrefab(PoiPrefabName prefabName)
    {
        if (prefabName == PoiPrefabName.unset)
        {
            Debug.LogWarning("Attempted to get an unset PoiPrefab. Returning null");
            return null;
        }


        PoiAttributes attributes = null;

        foreach (GameObject prefab in _poiPrefabs)
        {
            attributes = prefab.GetComponent<PoiAttributes>();

            if (attributes == null)
                continue;

            if (attributes.PoiPrefabName() == prefabName)
                return prefab;
        }

        Debug.Log($"Failed to find '{prefabName}' poiPrefab in PoiData Resource. Returning null");
        return null;

    }
}
