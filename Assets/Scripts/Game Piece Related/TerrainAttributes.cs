using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BasicResourceType
{
    unset,
    Food,
    Production
}

public class TerrainAttributes : MonoBehaviour
{
    //Declarations
    [SerializeField] private string _name = "undefined";
    [SerializeField] private string _description = "undefined Description";
    [SerializeField] private int _foodOutputPerTurn = 0;
    [SerializeField] private int _ProductionOutputPerTurn = 0;


    //Monos



    //Internals




    //Externals
    public string Name() {  return _name; }
    public string Description() { return _description; }
    public int FoodOutputPerTurn() { return _foodOutputPerTurn; }
    public int WorkOutputPerTurn() { return _ProductionOutputPerTurn; }


}
