using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePieceDisplayer : MonoBehaviour
{
    //Declarations
    [SerializeField] private GameObject _unitSelectionDisplay;
    [SerializeField] private GameObject _poiSelectionDisplay;
    [SerializeField] private TerrainDisplayController _terrainDisplayController;

    private void Awake()
    {
        SelectionKeeper.SetGamePieceDisplayer(this);
    }

    public void SetTerrainDisplay(GamePiece piece)
    {
        if (piece == null)
        {
            _terrainDisplayController.HideTerrainDisplay();
            return;
        }

        TerrainAttributes attributes = piece.GetComponent<TerrainAttributes>();

        _terrainDisplayController.ShowTerrainDisplay(attributes.TerrainName(), attributes.Description(), attributes.FoodOutputPerTurn(), attributes.WorkOutputPerTurn()); 
    }

    public void HideDisplays()
    {
        _terrainDisplayController.HideTerrainDisplay();
    }
}
