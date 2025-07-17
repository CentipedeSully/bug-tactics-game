using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiPooler : MonoBehaviour
{
    //Declarations
    [SerializeField] private int _restockSize = 3;
    [SerializeField] private Transform _tuberFieldContainer;
    [SerializeField] private GameBoard _board;
    [SerializeField] private BagOfHolding _bagOfHolding;


    //Internals
    private void Restock(PoiPrefabName prefabType)
    {
        int createdStock = 0;

        while (createdStock < _restockSize)
        {
            //UnitCreationHelper.CreateNewUnit(prefabType);
            createdStock++;
        }
    }




    //Externals
    public GameObject SpawnNewTuberField()
    {
        return null;
    }

    public GameObject SpawnNewTuberField((int, int) position)
    {
        //ignore spawn request if the position is invalid
        if (!_board.GetGrid().IsCellInGrid(position.Item1, position.Item2))
        {
            Debug.LogWarning($"New TuberField Spawn Request Denied. Position {position} invalid. returning null");
            return null;
        }

        //capture the PoiGroup gamePiece at the given position
        GamePiece poiGroupPiece = _board.GetPieceOnPosition(position, GamePieceType.PointOfInterestGroup);

        //Create a new UnitGroup gamePiece on the position if a unitGroup doesn't already exist there
        if (poiGroupPiece == null)
        {
            //create a new gp on the provided position to house the unit
            _bagOfHolding.SpawnGamePiece(GamePieceType.PointOfInterestGroup, position);
            poiGroupPiece = _board.GetPieceOnPosition(position, GamePieceType.PointOfInterestGroup);

            //if we STILL cant find the gamePiece, then we tried our best. return null.
            if (poiGroupPiece == null)
            {
                Debug.LogWarning($"Failed to find, AND create a unitGroup gamepiece at the position {position}. " +
                    $"Aborting spawn of fresh tuberField. returning null");
                return null;
            }
        }

        if (_tuberFieldContainer.childCount == 0)
            Restock(PoiPrefabName.TuberField);

        if (_tuberFieldContainer.childCount == 0)
        {
            Debug.LogWarning($"Failed to restock 'TuberField' pois, for some reason. returning null");
            return null;
        }

        int pickIndex = _tuberFieldContainer.childCount - 1;
        GameObject poiObject = _tuberFieldContainer.GetChild(pickIndex).gameObject;

        //set the unitGroup gamePiece as the new parent and activate the human object
        poiObject.transform.parent = poiGroupPiece.transform;
        poiObject.SetActive(true);
        return poiObject;
    }


    public Transform TuberFieldContainer(){ return _tuberFieldContainer; }

}
