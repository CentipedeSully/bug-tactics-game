using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectionKeeper 
{
    private static GameObject _selection;
    private static IndicatorManager _indicatorManager;
    private static GamePieceDisplayer _pieceDisplayer;

    public delegate void SelectionEvent(GameObject selection);
    public static event SelectionEvent OnSelectionUpdated;






    public static void SetSelection(GameObject newSelection, (int,int) cell) 
    {

        //null selections are acceptable
        _selection = newSelection;

        if (newSelection != null)
        {
            //Debug.Log($"New Selection: {newSelection.name}");
            _indicatorManager.RemoveAllSelectionIndicators();
            _indicatorManager.PlaceSelectionIndicator(cell);

            GamePiece piece = _selection.GetComponent<GamePiece>();

            if ( piece.GamePieceType() == GamePieceType.Terrain)
            {
                _pieceDisplayer.SetTerrainDisplay(piece);
            }
        }
            
        else
        {
            //Debug.Log("CLEARED Selection");
            _indicatorManager.RemoveCellIndicator(cell);
            _pieceDisplayer.HideDisplays();
        }

        OnSelectionUpdated?.Invoke(_selection);
    } 

    public static void ClearSelection()
    {
        _selection = null;

        _indicatorManager.RemoveAllSelectionIndicators();
        _pieceDisplayer.HideDisplays();
        OnSelectionUpdated?.Invoke(null);
    }

    public static GameObject Selection() {  return _selection; }
    public static void SetIndicatorManager(IndicatorManager newManager) {  _indicatorManager = newManager; }
    public static void SetGamePieceDisplayer(GamePieceDisplayer displayer) {  _pieceDisplayer = displayer; }


}
