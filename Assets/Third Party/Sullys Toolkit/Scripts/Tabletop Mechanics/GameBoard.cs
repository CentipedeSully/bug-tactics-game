using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SullysToolkit.TableTop
{
    public enum GameBoardLayer
    {
        Unset,
        Units,
        PointsOfInterest,
        Terrain
    }

    public class GameBoard : MonoBehaviour
    {
        //Declarations
        [Header("Board Size")]
        [SerializeField] [Min(1)] private int _rows = 1;
        [SerializeField] [Min(1)] private int _columns = 1;
        [SerializeField] [Min(.1f)] private float _cellSize = .1f;
        [SerializeField] private List<GamePiece> _gamePiecesInPlay;
        private GridSystem<bool> _boardGrid;
        [SerializeField] private Transform _unitContainer;
        [SerializeField] private Transform _poiContainer;
        [SerializeField] private Transform _terrainContainer;

        [Tooltip("This spawns gamePieces and holds despawned game pieces. Very Important")]
        [SerializeField] private BagOfHolding _bagOfHolding;

        [Header("TurnSystem Settings")]
        [SerializeField] private TurnSystem _turnSystem;

        [Header("Debug Settings")]
        [SerializeField] private bool _isDebugActive;



        //Monobehaviours
        private void Awake()
        {
            InitializeGamePieceList();
            CreateBoardGrid();   
        }




        //Internal Utils
        private void CreateBoardGrid()
        {
            Vector3 boardOrigin = new Vector3(transform.position.x - (_columns * _cellSize) / 2, transform.position.y - _rows * _cellSize / 2, transform.position.z);
            _boardGrid = new GridSystem<bool>(_columns, _rows, _cellSize, boardOrigin, () => { return false; });
            _boardGrid.SetDebugDrawDuration(999);
            _boardGrid.SetDebugDrawing(true);
        }

        private void InitializeGamePieceList()
        {
            _gamePiecesInPlay = new List<GamePiece>();
        }

        private void SetGamePieceAsChild(GamePiece gamePiece)
        {
            switch (gamePiece.GamePieceType())
            {
                case GamePieceType.UnitGroup:
                    gamePiece.transform.SetParent(_unitContainer);
                    break;

                case GamePieceType.PointOfInterestGroup:
                    gamePiece.transform.SetParent(_poiContainer);
                    break;

                case GamePieceType.Terrain:
                    gamePiece.transform.SetParent(_terrainContainer);
                    break;


                default:
                    gamePiece.transform.SetParent(this.transform);
                    break;
            }
            
        }

        private void SubscribePieceToTurnSystem(GamePiece gamePiece)
        {
            if (_turnSystem != null)
            {
                ITurnListener turnListener = gamePiece.GetComponent<ITurnListener>();
                if (turnListener != null)
                    _turnSystem.AddTurnListener(turnListener);
            }
            
        }

        private void UnsubscribePieceFromTurnSystem(GamePiece gamePiece)
        {
            if (_turnSystem != null)
            {
                ITurnListener turnListener = gamePiece.GetComponent<ITurnListener>();
                if (turnListener != null)
                    _turnSystem.RemoveTurnListener(turnListener);
            }
            
        }



        //Getters, Setters, & Commands
        public GridSystem<bool> GetGrid()
        {
            return _boardGrid;
        }

        public int GetRowCount()
        {
            return _rows;
        }

        public int GetColumnCount()
        {
            return _columns;
        }

        public float GetCellSize()
        {
            return _cellSize;
        }

        public List<GamePiece> GetAllGamePiecesInPlay()
        {
            return _gamePiecesInPlay;
        }

        public List<GamePiece> GetPiecesInLayer(GameBoardLayer layer)
        {
            LogStatement($"Fetching all active gamePieces in layer {layer}...");
            List<GamePiece> specifiedGamePiecesList =
                (from gamePiece in _gamePiecesInPlay
                where gamePiece.BoardLayer() == layer
                select gamePiece).ToList();

            LogStatement($"Game Pieces of layer {layer} found: {specifiedGamePiecesList.Count}");
            return specifiedGamePiecesList;
        }

        public List<GamePiece> GetPiecesOnPosition((int,int) xyPosition)
        {
            LogStatement($"Fetching all actve gamePieces on position {xyPosition.Item1},{xyPosition.Item2}...");
            List<GamePiece> querydPieces =
                (from gamePiece in _gamePiecesInPlay
                 where gamePiece.GridPosition() == xyPosition
                 select gamePiece).ToList();

            LogStatement($"GamePieces at position {xyPosition.Item1},{xyPosition.Item2} found: {querydPieces.Count}");
            return querydPieces;
        }

        public GamePiece GetPieceOnPosition((int,int) xyPosition, GamePieceType type)
        {
            List<GamePiece> piecesOnPosition = GetPiecesOnPosition(xyPosition);
            foreach (GamePiece piece in piecesOnPosition)
            {
                if (piece.GamePieceType() == type)
                    return piece;
            }

            return null;

        }

        public bool AddPieceToBoardState(GamePiece newGamePiece, GameBoardLayer desiredLayer, (int, int) xyDesiredPosition)
        {
            //LogStatement($"Checking if adding {newGamePiece.gameObject.name} to position ({xyDesiredPosition.Item1},{xyDesiredPosition.Item2}) is a valid");

            bool _doesPositionExistOnBoard = _boardGrid.IsCellInGrid(xyDesiredPosition.Item1, xyDesiredPosition.Item2);
            bool _doesPieceAlreadyExistOnBoard = DoesGamePieceExistOnBoard(newGamePiece);
            bool _isPositionAlreadyOccupiedOnLayer = IsPositionOccupied(xyDesiredPosition, desiredLayer);

            if (!_doesPieceAlreadyExistOnBoard && !_isPositionAlreadyOccupiedOnLayer && _doesPositionExistOnBoard)
            {
                LogStatement($"Attempting to Add {newGamePiece.gameObject.name} to gameBoard...");
                SetGamePieceAsChild(newGamePiece);
                SubscribePieceToTurnSystem(newGamePiece);

                _gamePiecesInPlay.Add(newGamePiece);
                return true;
                
            }
            else
                LogStatement($"Cannot add {newGamePiece.gameObject.name} to position ({xyDesiredPosition.Item1},{xyDesiredPosition.Item2})");

            return false;
        }

        public bool RemovePieceFromBoardState(GamePiece gamePiece)
        {
            LogStatement($"Attempting Removal of {gamePiece.gameObject.name}...");
            if (_gamePiecesInPlay.Contains(gamePiece))
            {
                _gamePiecesInPlay.Remove(gamePiece);
                UnsubscribePieceFromTurnSystem(gamePiece);
                LogStatement($"{ gamePiece.gameObject.name} removal successful");
                return true;
            }
            else
                LogStatement($"{gamePiece.gameObject.name} not found on Gameboard");
            return false;
        }

        public bool IsPositionOccupied((int,int) xyPosition, GameBoardLayer layer)
        {
            LogStatement($"Checking Cell ({xyPosition.Item1},{xyPosition.Item2}) Occupancy...");
            List<GamePiece> possiblePieces = GetPiecesInLayer(layer);

            var occupancyQuery =
                from gamePiece in possiblePieces
                where gamePiece.GridPosition() == xyPosition
                select gamePiece;

            LogStatement($"Occupancies found: {occupancyQuery.Count()}");
            if (occupancyQuery.Count() > 0)
                return true;
            else return false;
        }

        public bool DoesGamePieceExistOnBoard(GamePiece gamePiece)
        {
            LogStatement($"Does {gamePiece.gameObject.name} Exist On Board: { _gamePiecesInPlay.Contains(gamePiece)}");
            return _gamePiecesInPlay.Contains(gamePiece);
        }



        //Debug Utils
        private void LogStatement(string statement)
        {
            if (_isDebugActive)
                Debug.Log($"{statement}");
        }
    }
}

