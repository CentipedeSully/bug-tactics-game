using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;




/// <summary>
/// Responsible for storing AND RESPAWNING despawned gamePieces.
/// 
/// SPAWNING:
/// All gamePieces (gps) that enter play are first requested from this object.
/// If this object receives a request to spawn a gp, but has no stored gps by the provided definition,
/// then it will request the creation of a new gp from the GpCreator, which'll add the new object to the bag (if it's a valid request).
/// If the gp got added successfully, then this script will remove the new gp from the bag and spawn it to the board.
/// 
/// DESPAWNING:
/// The Bag of Holding listens to every piece it spawns. When a gamePiece despawns, it'll raise its despawn event, which'll
/// notify the Bag of Holding to store the piece. The piece resets its state on its own, and will even reparent itself under the bag.
/// </summary>
public class BagOfHolding : MonoBehaviour
{
    //Declarations
    [SerializeField] private GameBoard _board;
    [Tooltip("How many new pieces should be stocked if the bag runs out of a requested gamePiece")]
    [SerializeField] private int _restockAmount = 3;
    [SerializeField] private Transform _unitGpContainer;
    [SerializeField] private Transform _poiGpContainer;
    [SerializeField] private Transform _terrainGpContainer;



    //Monobehaviours

    private void Awake()
    {
        GpCreationHelper.SetBagOfHolding(this);
    }



    //Internals
    private bool IsSpawnRequestValid(GamePieceType type, (int,int) position)
    {
        //Make sure the type is set
        if (type == GamePieceType.Unset)
        {
            Debug.LogWarning($"Spawn Request on {position} Denied. Request provided an UNSET type");
            return false;
        }

        //Make sure the spawn position isn't already occupied
        switch (type)
        {
            case GamePieceType.Terrain:
                if (_board.IsPositionOccupied(position, GameBoardLayer.Terrain))
                {
                    Debug.LogWarning($"Spawn Request Denied. Terrain position {position} already occupied");
                    return false;
                }    
                break;

            case GamePieceType.PointOfInterestGroup:
                if (_board.IsPositionOccupied(position, GameBoardLayer.PointsOfInterest))
                {
                    Debug.LogWarning($"Spawn Request Denied. Poi position {position} already occupied");
                    return false;
                }
                break;

            case GamePieceType.UnitGroup:
                if (_board.IsPositionOccupied(position, GameBoardLayer.Units))
                {
                    Debug.LogWarning($"Spawn Request Denied. Unit position {position} already occupied");
                    return false;
                }
                break;
        }

        return true;

    }

    private void Restock(GamePieceType type)
    {

        int stock = 0;
        while (stock < _restockAmount)
        {
            GpCreationHelper.CreateGamePiece(type);
            stock++;
        }

    }
    




    //Externals
    public void StoreGamePiece(GameObject removedGamePiece)
    {
        if (removedGamePiece != null)
        {
            GamePieceType type= removedGamePiece.GetComponent<GamePiece>().GamePieceType();

            //check if the gamePiece is valid
            if (type == GamePieceType.Unset)
            {
                Debug.LogWarning($"Attempted to store a gamePiece of Unset type (gameObject: {removedGamePiece.name})");
                return;
            }

            //add the gamePiece to the appropriate collection
            switch (type)
            {
                case GamePieceType.Terrain:
                    removedGamePiece.transform.parent = _terrainGpContainer;
                    break;


                case GamePieceType.PointOfInterestGroup:
                    removedGamePiece.transform.parent = _poiGpContainer;
                    break;


                case GamePieceType.UnitGroup:
                    removedGamePiece.transform.parent = _unitGpContainer;
                    break;

            }

            //move the piece to it's container's origin
            removedGamePiece.transform.localPosition = Vector3.zero;
        }
    }

    public void SpawnGamePiece(GamePieceType type, (int,int) position)
    {
        //ignore the request if it's invalid
        if (!IsSpawnRequestValid(type, position))
            return;

        GameObject pieceObject = null;
        int childCount = 0;

        //Check the appropriate collection
        switch (type)
        {
            case GamePieceType.Terrain:

                childCount = _terrainGpContainer.childCount;

                //Restock if we're out
                if (childCount == 0)
                {
                    Restock(GamePieceType.Terrain);
                    childCount = _terrainGpContainer.childCount;
                }
                    

                //halt if we've failed to restock the requested piece
                if (childCount == 0)
                {
                    Debug.LogWarning($"Failed to Restock {type} gamePiece");
                    return;
                }

                //Select the latest gamePiece to enter the bag
                pieceObject = _terrainGpContainer.GetChild(childCount -1).gameObject;

                //spawn the removed object
                //the Spawn method auto the gamePiece to the gameBoard,
                //so there's no need to remove the child from the container here
                pieceObject.GetComponent<GamePiece>().Spawn(GameBoardLayer.Terrain, position); 
                break;


            case GamePieceType.PointOfInterestGroup:

                childCount = _poiGpContainer.childCount;

                //Restock if we're out
                if (childCount == 0)
                {
                    Restock(GamePieceType.PointOfInterestGroup);
                    childCount = _poiGpContainer.childCount;
                }


                //halt if we've failed to restock the requested piece
                if (childCount == 0)
                {
                    Debug.LogWarning($"Failed to Restock {type} gamePiece");
                    return;
                }

                //Select the latest gamePiece to enter the bag
                pieceObject = _poiGpContainer.GetChild(childCount - 1).gameObject;

                //spawn the removed object
                //the Spawn method auto the gamePiece to the gameBoard,
                //so there's no need to remove the child from the container here
                pieceObject.GetComponent<GamePiece>().Spawn(GameBoardLayer.PointsOfInterest, position);
                break;


            case GamePieceType.UnitGroup:

                childCount = _unitGpContainer.childCount;

                //Restock if we're out
                if (childCount == 0)
                {
                    Restock(GamePieceType.UnitGroup);
                    childCount = _unitGpContainer.childCount;
                }


                //halt if we've failed to restock the requested piece
                if (childCount == 0)
                {
                    Debug.LogWarning($"Failed to Restock {type} gamePiece");
                    return;
                }

                //Select the latest gamePiece to enter the bag
                pieceObject = _unitGpContainer.GetChild(childCount - 1).gameObject;

                //spawn the removed object
                //the Spawn method auto the gamePiece to the gameBoard,
                //so there's no need to remove the child from the container here
                pieceObject.GetComponent<GamePiece>().Spawn(GameBoardLayer.Units, position);
                break;

        }

    }

    public GameBoard GameBoard() { return _board; }

}
