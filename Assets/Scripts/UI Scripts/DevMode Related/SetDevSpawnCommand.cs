using SullysToolkit;
using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDevSpawnCommand : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private GamePieceType _type;
    [SerializeField] private bool _showLogs = true;

    public void EnterSpawnDevCommand()
    {
        if (DevCommandTracker.DevModeActive())
        {
            DevCommandTracker.EnterSpawnMode(_name,_type);
            STKDebugLogger.LogStatement(_showLogs, $"Current DevCmd: {DevCommandTracker.CurrentCommand()}, Name: {DevCommandTracker.GetSpecifiedName()},{DevCommandTracker.GetSpecifiedType()}");
        }
            
    }
}
