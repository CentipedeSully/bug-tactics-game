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
    private GameObject FetchPrefab(string name, GamePieceType type)
    {
        if (_bagOfHolding == null)
        {
            Debug.LogError("Can't Create gamePieces without a designated gameBag");
            return null;
        }

        if (_gpData == null)
        {
            Debug.LogError($"Missing a GamePieceData reference. Failed to create gamepiece '{name}' of type '{type}'");
            return null;
        }

        GameObject foundPrefab = null;

        switch (type)
        {
            case GamePieceType.Terrain:
                foundPrefab = _gpData.GetTerrain(name);
                break;
            case GamePieceType.PointOfInterest:
                foundPrefab = _gpData.GetPointOfInterest(name);
                break;
            case GamePieceType.Unit:
                foundPrefab = _gpData.GetUnit(name);
                break;

            default:
                Debug.LogError($"Failed to create gamePiece of '{type}' type");
                break;
        }


        return foundPrefab;
    }
    private bool DoesPrefabExist(string name, GamePieceType type)
    {
        GameObject requestedPrefab = FetchPrefab(name, type);
        return requestedPrefab != null;
    }




    //Externals
    public bool DoesPieceExist(string name, GamePieceType type) { return DoesPrefabExist(name, type); }
    public void CreateNewGamePiece(string prefabName, GamePieceType type)
    {
        GameObject prefab = FetchPrefab(prefabName, type);

        if (prefab != null)
        {
            //Create and Init the piece
            GameObject newGpObject = Instantiate(prefab);
            newGpObject.name = prefabName; //Make sure the Instance isn't suffixed with "Clone"
            newGpObject.GetComponent<GamePiece>().InitializeGamePiece(_bagOfHolding,_bagOfHolding.GameBoard());
            
            return;
        }

        Debug.Log($"Failed to fetch the prefab '{prefabName}' of type {type}");
    }
}
