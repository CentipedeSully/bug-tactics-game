using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum DevCommandState
{
    unset,
    SpawnObject,
    DespawnObject,
    DamageUnit
}
public static class DevCommandTracker
{
    //Declarations
    private static bool _devMode = true;
    private static DevCommandState _currentCommand;
    private static string _specifiedName;
    private static GamePieceType _specifiedType;
    private static int _specifiedValue;

    public delegate void DevModeEvent();
    public static event DevModeEvent OnDevModeEntered;
    public static event DevModeEvent OnDevModeExited;

    public delegate void DevCommandEvent(DevCommandState newState);
    public static event DevCommandEvent OnCommandStateEntered;




    //Internals
    private static void ClearCommandUtils()
    {
        _currentCommand = DevCommandState.unset;
        _specifiedName = "";
        _specifiedType = GamePieceType.Unset;
        _specifiedValue = 0;
    }



    //Externals
    public static bool DevModeActive() { return _devMode; }
    public static DevCommandState CurrentCommand() { return _currentCommand; }
    public static GamePieceType GetSpecifiedType() {  return _specifiedType; }
    public static string GetSpecifiedName() {  return _specifiedName; }
    public static int GetSpecifiedValue() { return _specifiedValue; }

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
            //Debug.Log("Entered DevMode");
            OnDevModeEntered?.Invoke();
        }
            
        else
        {
            //Debug.Log("Exited DevMode");
            OnDevModeExited?.Invoke();
        }
    }
    public static void ClearCurrentCommand()
    {
        ClearCommandUtils();
        OnCommandStateEntered?.Invoke(_currentCommand);
    }
    public static void EnterSpawnMode(string name, GamePieceType type)
    {
        if (_currentCommand != DevCommandState.SpawnObject)
        {
            ClearCommandUtils();

            _currentCommand = DevCommandState.SpawnObject;
        }

        _specifiedName = name;
        _specifiedType = type;

        OnCommandStateEntered?.Invoke(_currentCommand);
    }
    public static void EnterDespawnMode(GamePieceType targetType)
    {
        if (_currentCommand != DevCommandState.DespawnObject)
        {
            ClearCommandUtils();

            _currentCommand = DevCommandState.DespawnObject;
        }

        _specifiedType = targetType;

        OnCommandStateEntered?.Invoke(_currentCommand);
    }
    public static void EnterDamageUnitMode(int value)
    {
        if (_currentCommand != DevCommandState.DamageUnit)
        {
            ClearCommandUtils();

            _currentCommand = DevCommandState.DamageUnit;
        }

        _specifiedValue = value;

        OnCommandStateEntered?.Invoke(_currentCommand);
    }
}
