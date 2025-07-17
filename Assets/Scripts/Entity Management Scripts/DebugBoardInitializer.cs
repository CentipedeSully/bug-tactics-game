using SullysToolkit.TableTop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoardInitializer : MonoBehaviour
{
    //Declarations
    [SerializeField] private GameBoard _board;
    [SerializeField] private BagOfHolding _bagOfHolding;
    [SerializeField] private string _desiredTerrainStart;


    //Monobehaviours
    private void Start()
    {
        SpawnTiles();
    }



    //Internals
    private void SpawnTiles()
    {
        if (_board != null)
        {
            //make sure 
            int columns = _board.GetColumnCount();
            int rows = _board.GetRowCount();

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    //spawn a terrain at (i,j) position
                    //_bagOfHolding.SpawnGamePiece(_desiredTerrainStart, GamePieceType.Terrain, (i, j));
                }
            }
        }
    }



    //Externals




}
