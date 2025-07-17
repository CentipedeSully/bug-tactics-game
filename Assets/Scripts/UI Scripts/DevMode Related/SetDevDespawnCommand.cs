using SullysToolkit;
using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDevDespawnCommand : MonoBehaviour
{
    [SerializeField] private GamePieceType _targetType;
    [SerializeField] private bool _showLogs = true;


    public void EnterDespawnMode()
    {
        if (DevCommandTracker.DevModeActive())
        {
            DevCommandTracker.EnterDespawnMode(_targetType);
            STKDebugLogger.LogStatement(_showLogs, $"Current DevCmd: {DevCommandTracker.CurrentCommand()}, Target Type: {DevCommandTracker.GetGamePieceType()}");
        }
            
    }
}
