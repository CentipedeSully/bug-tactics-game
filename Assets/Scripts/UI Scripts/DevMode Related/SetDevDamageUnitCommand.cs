using SullysToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDevDamageUnitCommand : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private bool _showLogs;


    public void EnterDamageUnit()
    {
        if (DevCommandTracker.DevModeActive())
        {
            DevCommandTracker.EnterDamageUnitMode(_damage);
            STKDebugLogger.LogStatement(_showLogs, $"Current DevCmd: {DevCommandTracker.CurrentCommand()}, Target Type: {DevCommandTracker.GetSpecifiedType()}");
        }

    }
}
