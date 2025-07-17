using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiCreator : MonoBehaviour
{
    //Declarations
    [SerializeField] private PoiPooler _pooler;
    [SerializeField] private PoiData _data;
    private Transform _tuberfieldContainer;


    //monobehaviours
    private void Awake()
    {
        PoiCreationHelper.SetCreator(this);
    }

    //Externals
    public void CreateNewPoi(PoiPrefabName prefabName)
    {
        switch (prefabName)
        {
            case PoiPrefabName.TuberField:
                CreateTuberField();
                break;

            default:
                Debug.LogWarning($"No creation method for Poi with prefabName '{prefabName}'");
                break;
        }
    }

    public void CreateTuberField()
    {
        if (_pooler == null)
        {
            Debug.LogError("Missing PoiPooler reference");
            return;
        }

        if (_tuberfieldContainer == null)
        {
            if (_pooler.TuberFieldContainer() == null)
            {
                Debug.LogError("PoiPooler Missing reference to TuberFieldContainer");
                return;
            }

            _tuberfieldContainer = _pooler.TuberFieldContainer();
        }

        if (_data == null)
        {
            Debug.LogError("Missing PoiData Reference");
            return;
        }


        GameObject poiPrefab = _data.GetPoiPrefab(PoiPrefabName.TuberField);

        if (poiPrefab == null)
            return; //the Data object will Warn if it could'nt retrieve the prefab


        GameObject newPoi = Instantiate(poiPrefab, _tuberfieldContainer);
        newPoi.SetActive(false);
        return;
    }



}
