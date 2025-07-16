using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPooler : MonoBehaviour
{
    //Declarations
    [SerializeField] private int _restockSize = 3;
    [SerializeField] private Transform _humanContainer;
    [SerializeField] private Transform _grubContainer;



    //Internals
    private void Restock(UnitPrefabName prefabType)
    {
        int createdStock = 0;

        while (createdStock < _restockSize)
        {
            UnitCreationHelper.CreateNewUnit(prefabType);
            createdStock++;
        }
    }




    //Externals
    public GameObject SpawnNewHuman(Transform newParentContainer)
    {
        if (_humanContainer.childCount == 0)
            Restock(UnitPrefabName.human);

        if (_humanContainer.childCount == 0)
        {
            Debug.LogWarning($"Failed to restock 'Human' units, for some reason. returning null");
            return null;
        }

        int pickIndex = _humanContainer.childCount - 1;
        GameObject humanObject = _humanContainer.GetChild( pickIndex ).gameObject;

        //set the new parent and activate
        humanObject.transform.parent = newParentContainer;
        humanObject.SetActive( true);
        return humanObject;
    }

    public GameObject SpawnNewGrub(Transform newParentContainer)
    {
        if (_grubContainer.childCount == 0)
            Restock(UnitPrefabName.grub);

        if (_grubContainer.childCount == 0)
        {
            Debug.LogWarning($"Failed to restock 'grub' units, for some reason. returning null");
            return null;
        }

        int pickIndex = _grubContainer.childCount - 1;
        GameObject grubObject = _grubContainer.GetChild(pickIndex).gameObject;

        //set the new parent and activate
        grubObject.transform.parent = newParentContainer;
        grubObject.SetActive(true);
        return grubObject;
    }


    public Transform HumanContainer() { return _humanContainer; }
    public Transform GrubContainer() { return _grubContainer; }


}
