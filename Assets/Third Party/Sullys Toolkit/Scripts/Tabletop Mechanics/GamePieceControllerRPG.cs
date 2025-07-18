using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace SullysToolkit.TableTop.RPG
{
    public class GamePieceControllerRPG : MonoBehaviour, ITurnListener
    {
        //Declarations
        [Header("Controller Settings")]
        [SerializeField] private TurnPhase _controlsUnlockedPhase;
        [SerializeField] private bool _isControlAvailable = false;

        [Header("Cast & Detection Settings")]
        [SerializeField] private bool _isCasterReady = true;
        [SerializeField] private float _casterCooldown = .35f;
        [Tooltip("The first element takes priority over what will get shown. " +
            "If the gamepiece of that type doesn't exist, then the gamepiece of the next proiority will be displayed instead.")]
        [SerializeField] private List<GamePieceType> _displayPriorityList;
        [SerializeField] [Min(-1)] private int _selectedPositionX = -1;
        [SerializeField] [Min(-1)] private int _selectedPositionY = -1;
        [SerializeField] [Min(-1)] private int _hoveredPositionX = -1;
        [SerializeField] [Min(-1)] private int _hoveredPositionY = -1;
        [SerializeField] private GamePiece _unitOnCastPosition;
        [SerializeField] private GamePiece _poiOnCastPosition;
        [SerializeField] private GamePiece _terrainOnCastPosition;




        [Header("References")]
        [SerializeField] private ReadInput _inputReader;
        [SerializeField] private TurnSystem _turnSystem;
        [SerializeField] private GameBoard _gameBoard;
        [SerializeField] private BagOfHolding _bagOfHolding;
        [SerializeField] private MouseToWorld2D _mouseToWorldTrackerRef;
        [SerializeField] private GameObject _hoverIndicator;
        [SerializeField] private IndicatorManager _indicatorManager;

        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive = false;


        //Events



        //Monobehaviours
        private void Awake()
        {
            SelectionKeeper.SetIndicatorManager(_indicatorManager);
        }
        private void Update()
        {
            UpdateHoverPosition();
            ListenForInputCommands();
        }


        //Internal Utils
        private void UpdateHoverPosition()
        {
            (int, int) xyHoverPosition = _gameBoard.GetGrid().GetCellFromPosition(_mouseToWorldTrackerRef.GetWorldPosition());

            if (xyHoverPosition != (-1, -1))
            {
                _hoveredPositionX = xyHoverPosition.Item1;
                _hoveredPositionY = xyHoverPosition.Item2;
            }
            else
            {
                _hoveredPositionX = -1;
                _hoveredPositionY = -1;
            }

            if (_hoveredPositionX != -1 && _hoveredPositionY != -1)
            {
                _hoverIndicator.transform.position = _gameBoard.GetGrid().GetPositionFromCell(_hoveredPositionX, _hoveredPositionY);
                _hoverIndicator.SetActive(true);
            }
            else
                _hoverIndicator.SetActive(false);
        }

        private void ListenForInputCommands()
        {
            


            //Capture all game Pieces on the mouse's position
            if (_inputReader.LeftClick() && _isCasterReady)
            {
                
                (int, int) xySelectedPosition = _gameBoard.GetGrid().GetCellFromPosition(_mouseToWorldTrackerRef.GetWorldPosition());

                if (xySelectedPosition != (-1, -1))
                {
                    CooldownCaster();


                    //Perform the current DevCommand if any is active
                    if (IsDevCommandLoaded())
                    {
                        ExecuteDevCommand(xySelectedPosition);
                        return;
                    }

                    //Capture all pieces on the cell
                    CaptureSelectionViaMousePosition();

                    //Pick the gamePiece type that's the highest ptiority
                    GamePiece priorityPiece = GetHighestPrioritySelection();

                    //Set the captured piece as the new selection
                    if (priorityPiece != null)
                        SelectionKeeper.SetSelection(priorityPiece.gameObject,xySelectedPosition);
                    else SelectionKeeper.SetSelection(null, xySelectedPosition);

                    //Visualize where we just clicked on the board
                    _indicatorManager.PlaceClickIndicator(xySelectedPosition);
                }

            }

            //Clear the Current capture data
            else if (_inputReader.RightClick() && _isCasterReady)
            {
                CooldownCaster();


                //Clear the current dev command, if any is active
                if (IsDevCommandLoaded())
                    DevCommandTracker.ClearCurrentCommand();

                //Clear the current selection, if one exists
                if (SelectionKeeper.Selection() != null)
                    SelectionKeeper.ClearSelection();
            }
        }




        private void CaptureSelectionViaMousePosition()
        {
            (int, int) xySelection = GetCellPositionFromMouse();
            _selectedPositionX = xySelection.Item1;
            _selectedPositionY = xySelection.Item2;

            _unitOnCastPosition = GetUnitOnPosition(_selectedPositionX, _selectedPositionY);
            _poiOnCastPosition = GetPoiOnPosition(_selectedPositionX, _selectedPositionY);
            _terrainOnCastPosition = GetTerrainOnPosition(_selectedPositionX, _selectedPositionY);
        }

        private GamePiece GetHighestPrioritySelection()
        {
            //STKDebugLogger.LogStatement(_isDebugActive, $"Determining the non-null selection of highest priority...");
            for (int i = 0; i < _displayPriorityList.Count; i++)
            {

                if (_displayPriorityList[i] == GamePieceType.UnitGroup && _unitOnCastPosition != null)
                    return _unitOnCastPosition;

                else if (_displayPriorityList[i] == GamePieceType.PointOfInterestGroup && _poiOnCastPosition != null)
                    return _poiOnCastPosition;

                else if (_displayPriorityList[i] == GamePieceType.Terrain && _terrainOnCastPosition != null)
                    return _terrainOnCastPosition;
            }

            //STKDebugLogger.LogStatement(_isDebugActive, $"No Selection exists. Returning null");
            return null;
        }

        private (int,int) GetCellPositionFromMouse()
        {
            STKDebugLogger.LogStatement(_isDebugActive, "Capturing Grid Selection via Mouse Position...");
            Vector3 mousePosition = _mouseToWorldTrackerRef.GetWorldPosition();
            if (_gameBoard.GetGrid().IsPositionOnGrid(mousePosition))
            {
                (int, int) xySelectedPosition = _gameBoard.GetGrid().GetCellFromPosition(mousePosition);

                STKDebugLogger.LogStatement(_isDebugActive, $"Selected Position ({xySelectedPosition.Item1},{xySelectedPosition.Item2})");
                return xySelectedPosition;
            }

            else
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"No Position found. Mouse isn't on Game Grid. Returning (-1,-1)");
                return (-1, -1);
            }
        }

        private GamePiece GetUnitOnPosition(int x, int y)
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Getting unit on position ({x},{y})...");
            List<GamePiece> allPiecesOnPosition = _gameBoard.GetPiecesOnPosition((x,y));

            IEnumerable<GamePiece> unitQuery =
                from piece in allPiecesOnPosition
                where piece.GamePieceType() == GamePieceType.UnitGroup
                select piece;

            if (unitQuery.Any())
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"Unit Detected: {unitQuery.First().name}, ID: {unitQuery.First().GetInstanceID()}");
                return unitQuery.First();
            }
                
            else
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"No unit Detected");
                return null;
            }
        }

        private GamePiece GetPoiOnPosition(int x, int y)
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Getting PoI on position ({x},{y})...");
            List<GamePiece> allPiecesOnPosition = _gameBoard.GetPiecesOnPosition((x, y));

            IEnumerable<GamePiece> poiQuery =
                from piece in allPiecesOnPosition
                where piece.GamePieceType() == GamePieceType.PointOfInterestGroup
                select piece;

            if (poiQuery.Any())
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"PoI Detected: {poiQuery.First().name}, ID: {poiQuery.First().GetInstanceID()}");
                return poiQuery.First();
            }

            else
            {
                STKDebugLogger.LogStatement(_isDebugActive, $"No PoI Detected");
                return null;
            }
        }

        private GamePiece GetTerrainOnPosition(int x, int y)
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Getting Terrain on position ({x},{y})...");
            List<GamePiece> allPiecesOnPosition = _gameBoard.GetPiecesOnPosition((x, y));
            //Debug.Log("All Pieces on Position: " + allPiecesOnPosition.Count);

            foreach(GamePiece gp in allPiecesOnPosition)
            {
                //Debug.Log($"Checking if {gp.name} is a terrain");
                if (gp.GamePieceType() == GamePieceType.Terrain)
                {
                    //Debug.Log($"It is. returning {gp.name}");
                    return gp;
                }

                //Debug.Log($"Not a Terrain piece");
            }

            return null;
        }

        private void CooldownCaster()
        {
            _isCasterReady = false;
            Invoke(nameof(ReadyCaster), _casterCooldown);
        }

        private void ReadyCaster()
        {
            _isCasterReady = true;
        }

        private bool IsDevCommandLoaded()
        {
            return DevCommandTracker.DevModeActive() && DevCommandTracker.CurrentCommand() != DevCommandState.unset;
        }

        private void ExecuteDevCommand((int,int)targetPosition)
        {
            switch (DevCommandTracker.CurrentCommand())
            {
                //SPAWN SPECIFIED GP
                case DevCommandState.SpawnObject:
                    //_bagOfHolding.SpawnGamePiece(DevCommandTracker.GetSpecifiedUnitPrefab(), DevCommandTracker.GetGamePieceType(), targetPosition);
                    break;



                //DESPAWN SPECIFIED GP
                case DevCommandState.DespawnObject:

                    CaptureSelectionViaMousePosition();

                    switch (DevCommandTracker.GetGamePieceType())
                    {
                        case GamePieceType.Terrain:
                            if (_terrainOnCastPosition != null)
                                _terrainOnCastPosition.Despawn();
                            break;

                        case GamePieceType.PointOfInterestGroup:
                            if (_poiOnCastPosition != null)
                                _poiOnCastPosition.Despawn();
                            break;

                        case GamePieceType.UnitGroup:
                            if (_unitOnCastPosition != null)
                                _unitOnCastPosition.Despawn();
                            break;

                    }

                    break;



                //DAMAGE SPECIFIED GP
                case DevCommandState.DamageUnit:

                    CaptureSelectionViaMousePosition();
                    if (_unitOnCastPosition != null)
                        _unitOnCastPosition.GetComponent<UnitAttributes>().TakeDamage(DevCommandTracker.GetSpecifiedValue());
                    break;



            }
        }


        //Getters, Setters, & Commands
        public bool IsDebugActive()
        {
            return _isDebugActive;
        }

        public bool IsControlAvailable()
        {
            return _isControlAvailable;
        }

        public int GetResponsePhase()
        {
            return (int)_controlsUnlockedPhase;
        }

        public bool IsTurnListenerReadyToPassPhase()
        {
            return true;
        }

        public void RespondToNotification(int turnNumber)
        {
            _isControlAvailable = true;
        }

        public void ResetResponseFlag()
        {
            _isControlAvailable = false;
        }

        public ITurnBroadcaster GetTurnBroadcaster()
        {
            return _turnSystem;
        }

        public string GetConcreteListenerNameForDebugging()
        {
            return ToString() + ", ID:" + GetInstanceID();
        }

        public void ResetUtilsOnTurnSystemInterruption()
        {
            _isControlAvailable = false;
        }


        public List<GameObject> GetDetectedGpObjects()
        {
            List<GameObject> detectedPieces = new();
            if (_unitOnCastPosition != null)
                detectedPieces.Add(_unitOnCastPosition.gameObject);
            if (_poiOnCastPosition != null)
                detectedPieces.Add(_poiOnCastPosition.gameObject);
            if (_terrainOnCastPosition != null)
                detectedPieces.Add(_terrainOnCastPosition.gameObject);

            return detectedPieces;
        }
    }
}

