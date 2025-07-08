using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOnHealthZero : MonoBehaviour
{
    private GamePiece _gamePiece;
    private UnitAttributes _attributes;



    private void Awake()
    {
        _gamePiece = GetComponent<GamePiece>();
        _attributes = GetComponent<UnitAttributes>();
    }

    private void OnEnable()
    {
        _attributes.OnHealthZero += DespawnOnKO;
    }

    private void OnDisable()
    {
        _attributes.OnHealthZero -= DespawnOnKO;
    }


    private void DespawnOnKO()
    {
        Debug.Log($"Despawning gamePiece: {_gamePiece.GamePieceType()} '{gameObject.name}'");
        _gamePiece.Despawn();
    }
}
