using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SullysToolkit.TableTop
{
    public enum GamePieceType
    {
        Unset,
        Terrain,
        PointOfInterest,
        UnitGroup
    }

    public class GamePiece : MonoBehaviour
    {
        //Declarations
        [Header("Play Data")]
        [SerializeField] private GamePieceType _typeOfGamepiece;
        [SerializeField] private bool _isInitialized = false;
        [SerializeField] private bool _isInPlay;
        [SerializeField] private (int, int) _currentGridPosition = (-1, -1);

        [Header("GamePiece Metadata")]
        private BagOfHolding _bagOfHolding;
        private Transform _bagTransform;
        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private GameBoardLayer _boardLayer = GameBoardLayer.Unset;



        public delegate void GamePieceEvent(GamePiece self);
        public event GamePieceEvent OnCreation;
        public event GamePieceEvent OnEnteredPlay;
        public event GamePieceEvent OnExitedPlay;





        //Monobehaviours





        //Internal Utils
        private void MoveToBoardCell((int,int) xyBoardPosition)
        {
            transform.position = _gameBoard.GetGrid().GetPositionFromCell(xyBoardPosition.Item1, xyBoardPosition.Item2);
        }

        private bool RemoveFromPlay()
        {
            if (_isInPlay)
            {
                _gameBoard.RemovePieceFromBoardState(this);
                ClearBoardData();
                GoToBag();

                //signal exit, then disable the game object
                OnExitedPlay?.Invoke(this);
                gameObject.SetActive(false);
                return true;
            }

            Debug.LogWarning($"{gameObject.name} attempted to leave play as a gamePiece, but it isn't in play");
            return false;
        }

        private bool EnterPlay(GameBoardLayer layer, (int,int)position)
        {
            //On enter play if the fields are valid
            if (!_isInPlay && layer != GameBoardLayer.Unset && position != (-1,-1) && _isInitialized)
            {
                //set board data
                _isInPlay = true;
                _boardLayer = layer;
                _currentGridPosition = position;

                _gameBoard.AddPieceToBoardState(this,layer,position);
                MoveToBoardCell(position);

                //signal that we entered play, then enable the gameObject
                OnEnteredPlay?.Invoke(this);
                gameObject.SetActive(true);
                return true;

            }

            if (!_isInitialized)
            {
                Debug.LogWarning($"{gameObject.name} attempted to enter play without being initialized. " +
                                    $"Call 'InitializeGamePiece' to allow other potential creation scripts a chance to run " +
                                    $"before attempting to put the gamePiece on the board");
            }
            
            else 
                Debug.LogWarning($"{gameObject.name} attempted to enter play as a gamePiece, but detected invalid 'EnterPlay' parameter(s). Ignoring Request");
            return false;

        }

        private void ClearBoardData()
        {
            _isInPlay = false;
            _boardLayer = GameBoardLayer.Unset;
            _currentGridPosition = (-1, -1);
        }

        private void GoToBag()
        {
            transform.position = _bagTransform.position;
            transform.SetParent(_bagTransform);
            _bagOfHolding.StoreGamePiece(gameObject);
        }

       



        //Getters, Setters, & Commands
        public bool IsInPlay() { return _isInPlay; }

        public GamePieceType GamePieceType() { return _typeOfGamepiece; }

        public GameBoardLayer BoardLayer() { return _boardLayer; }

        public (int,int) GridPosition() { return _currentGridPosition; }

        public Transform OutOfPlayContainer() { return _bagTransform; }


        public bool InitializeGamePiece(BagOfHolding bagOfHolding, GameBoard board)
        {
            if (bagOfHolding != null && board != null)
            {
                _bagOfHolding = bagOfHolding;
                _bagTransform = bagOfHolding.transform; //used to despawn
                _gameBoard = board; //used to spawn

                //signal the creation event.
                //Lets any other dependent scripts init before going into the gamebag
                OnCreation?.Invoke(this);
                _isInitialized = true;
                GoToBag();
                gameObject.SetActive(false);
                return true;
            }
            
            Debug.Log($"{gameObject.name} failed to initialize as a gamePiece due to null bagOfHolding parameter. Ignoring Request");
            return false;
        }

        public void Spawn(GameBoardLayer layer, (int,int) position)
        {
            if (!_isInPlay)
            {
                _gameBoard.AddPieceToBoardState(this, layer, position);
                EnterPlay(layer, position);
                OnEnteredPlay?.Invoke(this);
            }
        }

        public void Despawn()
        {
            if (_isInPlay)
            {
                _gameBoard.RemovePieceFromBoardState(this);
                RemoveFromPlay();
                OnExitedPlay?.Invoke(this);
            }
        }

    }
}

