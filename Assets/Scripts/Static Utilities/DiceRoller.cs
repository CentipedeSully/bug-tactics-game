using System.Collections.Generic;
using UnityEngine;

public static class DiceRoller 
{
    public static int RollDie(int dieSize)
    {
        // return and invalid number if an impossible die is provided
        if (dieSize <= 0)
            return int.MinValue;

        // return 1 of die has only 1 side 
        if (dieSize == 1)
            return 1;


        return Random.Range(1, dieSize + 1);

    }

    public static int RollDice(int numberOfDice, int dieSize)
    {
        // return and invalid number if an impossible die is provided
        if (dieSize <= 0)
            return int.MinValue;

        
        int result = 0;

        for (int i = 0; i < numberOfDice; i++)
            result += RollDie(dieSize);

        return result;

    }
}
