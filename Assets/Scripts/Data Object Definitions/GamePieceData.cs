using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New GamePiece Data",menuName ="Data Objects/GamePiece Data")]
public class GamePieceData : ScriptableObject
{
    //Declarations
    [SerializeField] private GameObject _unitGroupPrefab;
    [SerializeField] private GameObject _poiGroupPrefab;
    [SerializeField] private GameObject _blankTerrainPrefab;




    //Monobehaviours





    //Internals






    //Externals
    public GameObject GetTerrainPrefab() { return _blankTerrainPrefab; }
    public GameObject GetPoiListPrefab() { return _poiGroupPrefab; }
    public GameObject GetUnitGroupPrefab() { return _unitGroupPrefab; }


}
