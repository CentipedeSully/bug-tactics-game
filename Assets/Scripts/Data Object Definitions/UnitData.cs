using System.Collections;
using System.Collections.Generic;
using UnityEngine;






public enum UnitPrefabName
{
    unset,
    human,
    grub
}

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Data Objects/Unit Data")]
public class UnitData : ScriptableObject
{
    //declarations
    [SerializeField] private List<GameObject> _unitPrefabs = new();







    //externals
    public GameObject GetUnitPrefab(UnitPrefabName unitType)
    {
        if (unitType == UnitPrefabName.unset)
        {
            Debug.LogWarning("Attempted to get an unset UnitPrefab. Returning null");
            return null;
        }

        UnitAttributes attributes = null;

        foreach (GameObject prefab in _unitPrefabs)
        {
            attributes = prefab.GetComponent<UnitAttributes>();

            if (attributes == null)
                continue;

            if (attributes.UnitPrefabName() == unitType)
                return prefab;
        }

        Debug.LogWarning($"Failed to find '{unitType}' unitType in UnitData Resource. Returning null");
        return null;
    }


}
