using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoiAttributes : MonoBehaviour
{
    //Declarations
    [SerializeField] private string _name = "unnamed Point of Interest";
    [SerializeField] private PoiPrefabName _prefabName = global::PoiPrefabName.unset;






    //Externals
    public string PoiName() { return _name; }
    public PoiPrefabName PoiPrefabName() { return _prefabName; }




}
