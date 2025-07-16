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
    [SerializeField] private TerrainPrefabName _prefabName = global::TerrainPrefabName.unset;
    [SerializeField] private string _name = "undefined";
    [SerializeField] private string _description = "undefined Description";
    [SerializeField] private int _foodOutputPerTurn = 0;
    [SerializeField] private int _ProductionOutputPerTurn = 0;


    //Monos



    //Internals




    //Externals
    public string TerrainName() {  return _name; }
    public TerrainPrefabName TerrainPrefabName() {  return _prefabName; }
    public string Description() { return _description; }
    public int FoodOutputPerTurn() { return _foodOutputPerTurn; }
    public int WorkOutputPerTurn() { return _ProductionOutputPerTurn; }


}
