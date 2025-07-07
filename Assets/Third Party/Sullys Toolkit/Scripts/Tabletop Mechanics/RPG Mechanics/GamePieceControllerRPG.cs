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
        [SerializeField] private MouseToWorld2D _mouseToWorldTrackerRef;

        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive = false;


        //Events



        //Monobehaviours

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
        }

        private void ListenForInputCommands()
        {
            


            //Capture all game Pieces on the mouse's position
            if (_inputReader.LeftClick() && _isCasterReady)
            {
                CooldownCaster();
                (int, int) xySelectedPosition = _gameBoard.GetGrid().GetCellFromPosition(_mouseToWorldTrackerRef.GetWorldPosition());

                if (xySelectedPosition != (-1, -1))
                {
                    //Perform the current DevCommand if any is active
                    if (IsDevCommandLoaded())
                    {
                        Debug.Log($"'{DevCommandTracker.CurrentCommand()}' executed (pretend it executed)");
                        //End this frame's progression
                        return;
                    }
                    


                    STKDebugLogger.LogStatement(_isDebugActive, $"Capturing new Selection data...");
                    ClearSelection();

                    CaptureSelectionViaMousePosition();
                    //Debug.Log($"Captured Pieces: \n{_unitOnCastPosition}\n{_poiOnCastPosition}\n{_terrainOnCastPosition}");
                }

            }

            //Clear the Current capture data
            else if (_inputReader.RightClick() && _isCasterReady)
            {
                CooldownCaster();


                //Clear the current dev command, if any is active
                if (IsDevCommandLoaded())
                    DevCommandTracker.ClearCurrentCommand();
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

        private void DisplaySelectedGamePieceOfHighestPriority()
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Selecting a gamePiece to display based on the current selection data & priority list...");
            for (int i = 0; i < _displayPriorityList.Count; i++)
            {

                if (_displayPriorityList[i] == GamePieceType.Unit && _unitOnCastPosition != null)
                {
                    DisplayGamePieceData(_unitOnCastPosition);
                    return;
                }

                else if (_displayPriorityList[i] == GamePieceType.PointOfInterest && _poiOnCastPosition != null)
                {
                    DisplayGamePieceData(_poiOnCastPosition);
                    return;
                }

                else if (_displayPriorityList[i] == GamePieceType.Terrain && _terrainOnCastPosition != null)
                {
                    DisplayGamePieceData(_terrainOnCastPosition);
                    return;
                }
            }

            STKDebugLogger.LogStatement(_isDebugActive, $"No selection exists to display");
        }

        private GamePiece GetHighestPrioritySelection()
        {
            STKDebugLogger.LogStatement(_isDebugActive, $"Determining the non-null selection of highest priority...");
            for (int i = 0; i < _displayPriorityList.Count; i++)
            {

                if (_displayPriorityList[i] == GamePieceType.Unit && _unitOnCastPosition != null)
                    return _unitOnCastPosition;

                else if (_displayPriorityList[i] == GamePieceType.PointOfInterest && _poiOnCastPosition != null)
                    return _poiOnCastPosition;

                else if (_displayPriorityList[i] == GamePieceType.Terrain && _terrainOnCastPosition != null)
                    return _terrainOnCastPosition;
            }

            STKDebugLogger.LogStatement(_isDebugActive, $"No Selection exists. Returning null");
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
                where piece.GetGamePieceType() == GamePieceType.Unit
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
                where piece.GetGamePieceType() == GamePieceType.PointOfInterest
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
                if (gp.GetGamePieceType() == GamePieceType.Terrain)
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

        private void DisplayGamePieceData(GamePiece piece)
        {
            IUIDisplayController displayController = piece?.GetComponent<IUIDisplayController>();

            if (displayController != null)
            {
                if (displayController.IsDataOnDisplay() == false)
                    displayController.DisplayData();

                displayController.UpdateData();
            }
                
        }

        private void HideGamePieceData(GamePiece piece)
        {
            IUIDisplayController displayController = piece?.GetComponent<IUIDisplayController>();

            if (displayController != null)
            {
                if (displayController.IsDataOnDisplay())
                    displayController.HideData();

                displayController.UpdateData();
            }
                
        }

        private bool IsDevCommandLoaded()
        {
            return DevCommandTracker.DevModeActive() && DevCommandTracker.CurrentCommand() != DevCommandState.unset;
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

        public void ClearSelection()
        {
            if (_unitOnCastPosition != null)
            {
                HideGamePieceData(_unitOnCastPosition);
                _unitOnCastPosition = null;
            }

            if (_poiOnCastPosition != null)
            {
                HideGamePieceData(_poiOnCastPosition);
                _poiOnCastPosition = null;
            }

            if (_terrainOnCastPosition != null)
            {
                HideGamePieceData(_terrainOnCastPosition);
                _terrainOnCastPosition = null;
            }
        }


        public GameObject GetDetectedPiece()
        {
            return GetHighestPrioritySelection().gameObject;
        }

        public void SetSelection(GameObject newSelection)
        {
            STKDebugLogger.LogWarning("Only selection via mouse are supported, currently");
        }

        public List<GameObject> GetSelectionCollection()
        {
            List<GameObject> returnList = new List<GameObject>();

            if (_unitOnCastPosition != null)
                returnList.Add(_unitOnCastPosition.gameObject);

            if (_poiOnCastPosition != null)
                returnList.Add(_poiOnCastPosition.gameObject);

            if (_terrainOnCastPosition != null)
                returnList.Add(_terrainOnCastPosition.gameObject);

            return returnList;
           
        }

        public void AddSelection(GameObject newSelection)
        {
            SetSelection(newSelection);
        }

        public void RemoveSelection(GameObject existingSelection)
        {
            ClearSelection();
        }

        public bool IsSelectionAvailable()
        {
            bool unitExists = _unitOnCastPosition != null;
            bool poiExists = _poiOnCastPosition != null;
            bool terrainExists = _terrainOnCastPosition != null;

            return unitExists || poiExists || terrainExists;
        }
    }
}

