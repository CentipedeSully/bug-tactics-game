using SullysToolkit.TableTop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardVisualizer : MonoBehaviour
{
    //Declarations
    [SerializeField] private GameBoard _board;
    [SerializeField] private GameObject _tilePrefab;


    //Monobehaviours
    private void Start()
    {
        SpawnTiles();
    }



    //Internals
    private void SpawnTiles()
    {
        if (_tilePrefab != null && _board != null)
        {
            int columns = _board.GetColumnCount();
            int rows = _board.GetRowCount();
            GameObject tileObject = null;
            GamePiece piece = null;

            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    tileObject = Instantiate(_tilePrefab);
                    piece = tileObject.GetComponent<GamePiece>();
                    _board.AddGamePiece(piece, GameBoardLayer.Terrain, (i, j));
                    tileObject.name = $"Terrain ({i},{j})";
                }
            }
        }
    }



    //Externals




}
