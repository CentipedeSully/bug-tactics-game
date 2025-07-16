using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitCreationHelper
{
    private static UnitCreator _creator;





    public static void SetUnitCreator(UnitCreator creator) { _creator = creator; }

    public static void CreateNewUnit(UnitPrefabName prefabName)
    {
        _creator.CreateNewUnit(prefabName);
    }
}
