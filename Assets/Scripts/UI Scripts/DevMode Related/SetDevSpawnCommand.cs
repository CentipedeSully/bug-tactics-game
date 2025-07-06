using SullysToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDevSpawnCommand : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private bool _showLogs = true;

    public void EnterSpawnDevCommand()
    {
        if (DevCommandTracker.DevModeActive())
        {
            DevCommandTracker.EnterSpawnMode(_prefab);
            STKDebugLogger.LogStatement(_showLogs, $"Current DevCmd: {DevCommandTracker.CurrentCommand()}, Prefab: {DevCommandTracker.GetSpawnObject()}");
        }
            
    }
}
