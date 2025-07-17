using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoiCreationHelper
{
    private static PoiCreator _creator;




    public static void SetCreator(PoiCreator creator) { _creator = creator; }
    public static void CreatePoi(PoiPrefabName prefabName)
    {
        _creator.CreateNewPoi(prefabName);
    }
}
