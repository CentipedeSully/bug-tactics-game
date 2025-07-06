using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum DevCommandState
{
    unset,
    SpawnObject,
    DespawnObject
}
public static class DevCommandTracker
{
    //Declarations
    private static bool _devMode = true;
    private static DevCommandState _currentCommand;
    private static GameObject _spawnObject;
    private static GamePieceType _despawnTarget;

    public delegate void DevModeEvent();
    public static event DevModeEvent OnDevModeEntered;
    public static event DevModeEvent OnDevModeExited;

    public delegate void DevCommandEvent(DevCommandState newState);
    public static event DevCommandEvent OnCommandStateEntered;




    //Internals
    private static void ClearCommandUtils()
    {
        _currentCommand = DevCommandState.unset;
        _spawnObject = null;
        _despawnTarget = GamePieceType.Unset;
    }



    //Externals
    public static bool DevModeActive() { return _devMode; }
    public static DevCommandState CurrentCommand() { return _currentCommand; }
    public static GameObject GetSpawnObject() {  return _spawnObject; }
    public static GamePieceType GetDespawnTarget() { return _despawnTarget; }

    public static void ExitDevMode()
    {
        if (_devMode)
            ToggleDevMode();
    }
    public static void ToggleDevMode()
    {
        _devMode = !_devMode;
        ClearCurrentCommand();

        if (_devMode)
        {
            Debug.Log("Entered DevMode");
            OnDevModeEntered?.Invoke();
        }
            
        else
        {
            Debug.Log("Exited DevMode");
            OnDevModeExited?.Invoke();
        }
    }
    public static void ClearCurrentCommand()
    {
        ClearCommandUtils();
        OnCommandStateEntered?.Invoke(_currentCommand);
    }
    public static void EnterSpawnMode(GameObject prefab)
    {
        if (_currentCommand != DevCommandState.SpawnObject)
        {
            ClearCommandUtils();

            _currentCommand = DevCommandState.SpawnObject;
        }

        _spawnObject = prefab;

        OnCommandStateEntered?.Invoke(_currentCommand);
    }
    public static void EnterDespawnMode(GamePieceType targetType)
    {
        if (_currentCommand != DevCommandState.DespawnObject)
        {
            ClearCommandUtils();

            _currentCommand = DevCommandState.DespawnObject;
        }

        _despawnTarget = targetType;

        OnCommandStateEntered?.Invoke(_currentCommand);
    }
}
