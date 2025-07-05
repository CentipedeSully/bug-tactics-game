using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollerDebugger : MonoBehaviour
{
    [SerializeField] private int _numberOfDice;
    [SerializeField] private int _dieSize;
    [SerializeField] private bool _rollDice;


    private void Update()
    {
        RollDice();
    }

    private void RollDice()
    {
        if (_rollDice)
        {
            _rollDice = false;
            int results = DiceRoller.RollDice(_numberOfDice, _dieSize);

            Debug.Log($"Rolling {_numberOfDice}d{_dieSize} => {results}");
        }
    }
}
