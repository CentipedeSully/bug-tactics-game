using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New GamePiece Data",menuName ="Data Objects/GamePiece Data")]
public class GamePieceData : ScriptableObject
{
    //Declarations
    [SerializeField] private List<GameObject> _units= new List<GameObject>();
    [SerializeField] private List<GameObject> _pointsOfInterest= new List<GameObject>();
    [SerializeField] private List<GameObject> _terrains= new List<GameObject>();




    //Monobehaviours





    //Internals






    //Externals
    public GameObject GetUnit(string name)
    {
        foreach (GameObject unit in _units)
        {
            if (unit.name == name)
                return unit;
        }

        return null;
    }

    public GameObject GetPointOfInterest(string name)
    {
        foreach (GameObject site in _pointsOfInterest)
        {
            if (site.name == name)
                return site;
        }

        return null;
    }

    public GameObject GetTerrain(string name)
    {
        foreach (GameObject terrain in _terrains)
        {
            if (terrain.name == name)
                return terrain;
        }

        return null;
    }


}
