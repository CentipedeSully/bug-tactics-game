using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum TerrainPrefabName
{
    unset,
    DebugLands
    
}


[CreateAssetMenu(fileName = "New Terrain Data", menuName = "Data Objects/Terrain Data")]
public class TerrainData : ScriptableObject
{
    [SerializeField] private List<GameObject> _terrainPrefabs = new List<GameObject>();






    public GameObject GetTerrainPrefab(TerrainPrefabName prefabName)
    {
        if (prefabName == TerrainPrefabName.unset)
        {
            Debug.LogWarning("Attempted to get an unset TerrainPrefab. Returning null");
            return null;
        }


        TerrainAttributes attributes = null;

        foreach (GameObject prefab in _terrainPrefabs)
        {
            attributes = prefab.GetComponent<TerrainAttributes>();

            if (attributes == null)
                continue;

            if (attributes.TerrainPrefabName() == prefabName)
                return prefab;
        }

        Debug.Log($"Failed to find '{prefabName}' terrainPrefab in TerrainData Resource. Returning null");
        return null;

    }
}
