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
    [SerializeField] private GpCreator _gpCreator;
    [Tooltip("How many new pieces should be stocked if the bag runs out of a requested gamePiece")]
    [SerializeField] private int _restockAmount = 3;
    private Dictionary<string, List<GameObject>> _inactiveTerrains;
    private Dictionary<string, List<GameObject>> _inactivePois;
    private Dictionary<string, List<GameObject>> _inactiveUnits;



    //Monobehaviours





    //Internals
    private bool IsSpawnRequestValid(string name, GamePieceType type, (int,int) position)
    {
        //Make sure the type is set
        if (type == GamePieceType.Unset)
        {
            Debug.LogError($"Spawn Request for '{name}' Denied. Request provided an UNSET type");
            return false;
        }

        //Make sure the spawn position isn't already occupied
        switch (type)
        {
            case GamePieceType.Terrain:
                if (_board.IsPositionOccupied(position, GameBoardLayer.Terrain))
                {
                    Debug.LogError($"Spawn Request for '{name}' Denied. Terrain position {position} already occupied");
                    return false;
                }    
                break;

            case GamePieceType.PointOfInterest:
                if (_board.IsPositionOccupied(position, GameBoardLayer.PointsOfInterest))
                {
                    Debug.LogError($"Spawn Request for '{name}' Denied. Poi position {position} already occupied");
                    return false;
                }
                break;

            case GamePieceType.Unit:
                if (_board.IsPositionOccupied(position, GameBoardLayer.Units))
                {
                    Debug.LogError($"Spawn Request for '{name}' Denied. Unit position {position} already occupied");
                    return false;
                }
                break;
        }

        //Make sure the request object exists in the dataObject
        if (!_gpCreator.DoesPieceExist(name, type))
        {
            Debug.LogError($"Spawn Request for {type} gamePiece '{name}' Denied. Requested gamePiece doesn't exist within the GamePieceData Asset");
            return false;
        }


        return true;

    }

    private void Restock(string name, GamePieceType type)
    {
        int stock = 0;
        while (stock < _restockAmount)
        {
            _gpCreator.CreateNewGamePiece(name, type);
            stock++;
        }
    }
    




    //Externals
    public void StoreGamePiece(GameObject removedGamePiece)
    {
        if (removedGamePiece != null)
        {
            Debug.Log("Before Storage  vvvvvvvvvvvvvvvvvvvvvv ");
            LogContents();

            string pieceName = gameObject.name;
            GamePieceType type= gameObject.GetComponent<GamePiece>().GamePieceType();

            //check if the gamePiece is valid
            if (type == GamePieceType.Unset)
            {
                Debug.LogError($"Attempted to store a gamePiece of Unset type ({pieceName})");
                return;
            }

            //add the gamePiece to the appropriate collection
            switch (type)
            {
                case GamePieceType.Terrain:
                    if (_inactiveTerrains.ContainsKey(pieceName))
                        _inactiveTerrains[pieceName].Add(removedGamePiece);

                    else
                    {
                        _inactiveTerrains[pieceName] = new List<GameObject>();
                        _inactiveTerrains[pieceName].Add(removedGamePiece);
                    }
                    break;


                case GamePieceType.PointOfInterest:
                    if (_inactivePois.ContainsKey(pieceName))
                        _inactivePois[pieceName].Add(removedGamePiece);

                    else
                    {
                        _inactivePois[pieceName] = new List<GameObject>();
                        _inactivePois[pieceName].Add(removedGamePiece);
                    }
                    break;


                case GamePieceType.Unit:
                    if (_inactiveUnits.ContainsKey(pieceName))
                        _inactiveUnits[pieceName].Add(removedGamePiece);

                    else
                    {
                        _inactiveUnits[pieceName] = new List<GameObject>();
                        _inactiveUnits[pieceName].Add(removedGamePiece);
                    }
                    break;

            }

            Debug.Log("After Storage ^^^^^^^^^^^^^^^^^^^^^^^ ");
            LogContents();
        }
    }

    public void SpawnGamePiece(string name, GamePieceType type, (int,int) position)
    {
        //ignore the request if it's invalid
        if (!IsSpawnRequestValid(name, type, position))
            return;

        GameObject pieceObject = null;

        //Check the appropriate collection
        switch (type)
        {
            case GamePieceType.Terrain:

                //Make sure the name exists as a subcategory
                if (!_inactiveTerrains.ContainsKey(name))
                    _inactiveTerrains[name] = new List<GameObject>();

                //Restock if we're out
                if (_inactiveTerrains[name].Count == 0)
                    Restock(name, GamePieceType.Terrain);

                //halt if we've failed to restock the requested piece
                if (_inactiveTerrains[name].Count == 0)
                {
                    Debug.LogError($"Failed to Restock {type} gamePiece: '{name}'");
                    return;
                }

                //remove a copy from the bag
                int lastTerrainIndex = _inactiveTerrains[name].Count - 1; //pick from the end to reduce reindexing
                pieceObject = _inactiveTerrains[name][lastTerrainIndex];
                _inactiveTerrains[name].RemoveAt(lastTerrainIndex);

                //spawn the removed object
                pieceObject.GetComponent<GamePiece>().Spawn(GameBoardLayer.Terrain, position);
                break;


            case GamePieceType.PointOfInterest:
                //Make sure the name exists as a subcategory
                if (!_inactivePois.ContainsKey(name))
                    _inactivePois[name] = new List<GameObject>();

                //Restock if we're out
                if (_inactivePois[name].Count == 0)
                    Restock(name, GamePieceType.PointOfInterest);

                //halt if we've failed to restock the requested piece
                if (_inactivePois[name].Count == 0)
                {
                    Debug.LogError($"Failed to Restock {type} gamePiece: '{name}'");
                    return;
                }

                //remove a copy from the bag
                int lastPoiIndex = _inactivePois[name].Count - 1; //pick from the end to reduce reindexing
                pieceObject = _inactivePois[name][lastPoiIndex];
                _inactivePois[name].RemoveAt(lastPoiIndex);

                //spawn the removed object
                pieceObject.GetComponent<GamePiece>().Spawn(GameBoardLayer.PointsOfInterest, position);
                break;


            case GamePieceType.Unit:
                //Make sure the name exists as a subcategory
                if (!_inactiveUnits.ContainsKey(name))
                    _inactiveUnits[name] = new List<GameObject>();

                //Restock if we're out
                if (_inactiveUnits[name].Count == 0)
                    Restock(name, GamePieceType.Unit);

                //halt if we've failed to restock the requested piece
                if (_inactiveUnits[name].Count == 0)
                {
                    Debug.LogError($"Failed to Restock {type} gamePiece: '{name}'");
                    return;
                }

                //remove a copy from the bag
                int lastUnitIndex = _inactiveUnits[name].Count - 1; //pick from the end to reduce reindexing
                pieceObject = _inactiveUnits[name][lastUnitIndex];
                _inactiveUnits[name].RemoveAt(lastUnitIndex);

                //spawn the removed object
                pieceObject.GetComponent<GamePiece>().Spawn(GameBoardLayer.Units, position);
                break;

        }

        
    }

    public GameBoard GameBoard() { return _board; }

    public void LogContents()
    {
        string log = "Bag of Holding Contents: \n=====================================\n";

        log += "UNIT PIECES:\n";

        if (_inactiveUnits.Count == 0)
            log += "[None]\n";
        else
        {
            foreach (KeyValuePair<string, List<GameObject>> entry in _inactiveUnits)
                log += $"{entry.Key}(s): {entry.Value.Count}\n";
        }



        log += "------------------------------\nPOINT OF INTEREST PIECES:\n";

        if (_inactivePois.Count == 0)
            log += "[None]\n";
        else
        {
            foreach (KeyValuePair<string, List<GameObject>> entry in _inactivePois)
                log += $"{entry.Key}(s): {entry.Value.Count}\n";
        }
        


        log += "------------------------------\nTERRAIN PIECES:\n";
        if (_inactiveTerrains.Count == 0)
            log += "[None]\n";
        else
        {
            foreach (KeyValuePair<string, List<GameObject>> entry in _inactiveTerrains)
                log += $"{entry.Key}(s): {entry.Value.Count}\n";
        }
       

        log += "\n============== CONTENT LOG END ================";
        Debug.Log(log);
    }



}
