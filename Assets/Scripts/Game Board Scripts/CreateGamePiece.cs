using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CreateGamePiece : MonoBehaviour
{
    //Declarations
    private GameBoard _board;
    [Tooltip("Where inactive game pieces live")]
    [SerializeField] private Transform _bagOfHolding;


    //monobehaviours
    private void Awake()
    {
        _board = GetComponent<GameBoard>();
    }



    //Internals
    private GameObject CreateUnitGamePiece()
    {
        return null;
    }




    //Externals
}
