using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    //Declarations
    [SerializeField] private UnitPooler _pooler;
    [SerializeField] private UnitData _data;
    private Transform _humanContainer;
    private Transform _grubContainer;


    //monobehaviours
    private void Awake()
    {
        UnitCreationHelper.SetUnitCreator(this);
    }

    //Externals
    public void CreateNewUnit(UnitPrefabName prefabName)
    {
        switch (prefabName)
        {
            case UnitPrefabName.human:
                CreateNewHuman();
                break;

                case UnitPrefabName.grub:
                CreateNewGrub();
                break;

            default:
                Debug.LogWarning($"No creation method for unit with prefabName '{prefabName}'");
                break;
        }
    }

    public void CreateNewHuman()
    {
        if (_pooler == null)
        {
            Debug.LogError("Missing UnitPooler reference");
            return;
        }

        if (_humanContainer == null)
        {
            if (_pooler.HumanContainer() == null)
            {
                Debug.LogError("UnitPooler Missing reference to HumanContainer");
                return;
            }

            _humanContainer = _pooler.HumanContainer();
        }

        if (_data == null)
        {
            Debug.LogError("Missing UnitData Reference");
            return;
        }


        GameObject unitPrefab = _data.GetUnitPrefab(UnitPrefabName.human);

        if (unitPrefab == null)
            return; //the Data object will Warn if it could'nt retrieve the prefab


        GameObject newUnit = Instantiate(unitPrefab,_humanContainer);
        newUnit.SetActive(false);
        return;
    }
    public void CreateNewGrub()
    {
        if (_pooler == null)
        {
            Debug.LogError("Missing UnitPooler reference");
            return;
        }

        if (_grubContainer == null)
        {
            if (_pooler.GrubContainer() == null)
            {
                Debug.LogError("UnitPooler Missing reference to GrubContainer");
                return;
            }

            _grubContainer = _pooler.GrubContainer();
        }

        if (_data == null)
        {
            Debug.LogError("Missing UnitData Reference");
            return;
        }


        GameObject unitPrefab = _data.GetUnitPrefab(UnitPrefabName.grub);

        if (unitPrefab == null)
            return; //the Data object will Warn if it could'nt retrieve the prefab


        GameObject newUnit = Instantiate(unitPrefab, _grubContainer);
        newUnit.SetActive(false);
        return;
    }





}
