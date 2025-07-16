using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GpCreator : MonoBehaviour
{
    //Declarations
    [SerializeField] private BagOfHolding _bagOfHolding;
    [SerializeField] private GamePieceData _gpData;







    //monobehaviours
    private void Awake()
    {
        GpCreationHelper.SetCreator(this);
    }



    //internals
    

    private GameObject FetchPrefab(GamePieceType type)
    {
        if (_bagOfHolding == null)
        {
            Debug.LogError("Can't Create gamePieces without a designated gameBag");
            return null;
        }

        if (_gpData == null)
        {
            Debug.LogError($"Missing a GamePieceData reference. Failed to create gamepiece of type '{type}'");
            return null;
        }

        GameObject foundPrefab = null;

        switch (type)
        {
            case GamePieceType.Terrain:
                foundPrefab = _gpData.GetTerrainPrefab();
                break;
            case GamePieceType.PointOfInterest:
                foundPrefab = _gpData.GetPoiListPrefab();
                break;
            case GamePieceType.UnitGroup:
                foundPrefab = _gpData.GetUnitGroupPrefab();
                break;

            default:
                Debug.LogError($"No creation method exists for gamePiece of '{type}' type");
                break;
        }


        return foundPrefab;
    }


    //Externals
    public void CreateNewGamePiece(GamePieceType type)
    {
        GameObject prefab = FetchPrefab(type);

        if (prefab != null)
        {
            //Create and Init the piece
            GameObject newGpObject = Instantiate(prefab);
            newGpObject.GetComponent<GamePiece>().InitializeGamePiece(_bagOfHolding,_bagOfHolding.GameBoard());
            
            return;
        }

        Debug.Log($"Failed to fetch the gamePiece prefab of type {type}");
    }
}
