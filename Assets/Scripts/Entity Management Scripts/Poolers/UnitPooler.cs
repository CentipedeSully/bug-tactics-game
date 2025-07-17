using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitPooler : MonoBehaviour
{
    //Declarations
    [SerializeField] private int _restockSize = 3;
    [SerializeField] private Transform _humanContainer;
    [SerializeField] private Transform _grubContainer;
    [SerializeField] private GameBoard _board;
    [SerializeField] private BagOfHolding _bagOfHolding;


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
    public GameObject SpawnNewHuman((int,int) position)
    {
        //ignore spawn request if the position is invalid
        if ( !_board.GetGrid().IsCellInGrid(position.Item1,position.Item2))
        {
            Debug.LogWarning($"New Human Spawn Request Denied. Position {position} invalid. returning null");
                return null;
        }

        //capture the unitGroup gamePiece at the given position
        GamePiece unitGroupPiece= _board.GetPieceOnPosition(position,GamePieceType.UnitGroup); 

        //Create a new UnitGroup gamePiece on the position if a unitGroup doesn't already exist there
        if (unitGroupPiece == null)
        {
            //create a new gp on the provided position to house the unit
            _bagOfHolding.SpawnGamePiece(GamePieceType.UnitGroup, position);
            unitGroupPiece = _board.GetPieceOnPosition(position, GamePieceType.UnitGroup);

            //if we STILL cant find the gamePiece, then we tried our best. return null.
            if (unitGroupPiece == null)
            {
                Debug.LogWarning($"Failed to find, AND create a unitGroup gamepiece at the position {position}. " +
                    $"Aborting spawn of fresh Human. returning null");
                return null;
            }
        }

        if (_humanContainer.childCount == 0)
            Restock(UnitPrefabName.human);

        if (_humanContainer.childCount == 0)
        {
            Debug.LogWarning($"Failed to restock 'Human' units, for some reason. returning null");
            return null;
        }

        int pickIndex = _humanContainer.childCount - 1;
        GameObject humanObject = _humanContainer.GetChild( pickIndex ).gameObject;

        //set the unitGroup gamePiece as the new parent and activate the human object
        humanObject.transform.parent = unitGroupPiece.transform;
        humanObject.SetActive( true);
        return humanObject;
    }

    public GameObject SpawnNewGrub((int,int) position)
    {
        //ignore spawn request if the position is invalid
        if (!_board.GetGrid().IsCellInGrid(position.Item1, position.Item2))
        {
            Debug.LogWarning($"New Grub Spawn Request Denied. Position {position} invalid. returning null");
            return null;
        }

        //capture the unitGroup gamePiece at the given position
        GamePiece unitGroupPiece = _board.GetPieceOnPosition(position, GamePieceType.UnitGroup);


        //Create a new unitGroup on the position if a unitGroup doesn't already exist there
        if (unitGroupPiece == null)
        {
            //create a new gp on the provided position to house the unit
            _bagOfHolding.SpawnGamePiece(GamePieceType.UnitGroup, position);
            unitGroupPiece = _board.GetPieceOnPosition(position, GamePieceType.UnitGroup);

            //if we STILL cant find the gamePiece, then we tried our best. return null.
            if (unitGroupPiece == null)
            {
                Debug.LogWarning($"Failed to find, AND create a unitGroup gamepiece at the position {position}. " +
                    $"Aborting spawn of fresh Grub. returning null");
                return null;
            }
        }

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
        grubObject.transform.parent = unitGroupPiece.transform;
        grubObject.SetActive(true);
        return grubObject;
    }


    public Transform HumanContainer() { return _humanContainer; }
    public Transform GrubContainer() { return _grubContainer; }


}
