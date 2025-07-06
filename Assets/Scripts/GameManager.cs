using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Declarations
    [SerializeField] private bool _devModeEnabled = true;
    [SerializeField] private bool _isDevModeActive = false;
    [SerializeField] private GameObject _devUi;
    [SerializeField] private GameObject _devMenu;



    //monobehaviours
    private void OnEnable()
    {
        if (_devModeEnabled)
        {
            DevCommandTracker.OnDevModeEntered += ShowDevMenu;
            DevCommandTracker.OnDevModeEntered += WatchDevMode;
            DevCommandTracker.OnDevModeExited += HideDevMenu;
            DevCommandTracker.OnDevModeExited += WatchDevMode;
        }

        //Clear Devmode, in case the last session didn't leave it
        DevCommandTracker.ExitDevMode();

        //hide the devmode Ui if it isn't enabled
        _devUi.SetActive(_devModeEnabled);

        
    }


    private void OnDisable()
    {
        //Clear Devmode, in case we didn't explicitly leave it yet
        DevCommandTracker.ExitDevMode();

        DevCommandTracker.OnDevModeEntered -= ShowDevMenu;
        DevCommandTracker.OnDevModeEntered -= WatchDevMode;
        DevCommandTracker.OnDevModeExited -= HideDevMenu;
        DevCommandTracker.OnDevModeExited -= WatchDevMode;
    }





    //internals
    private void HideDevMenu()
    {
        _devMenu.SetActive(false);
    }
    private void ShowDevMenu()
    {
        _devMenu.SetActive(true);
    }
    private void WatchDevMode()
    {
        _isDevModeActive = DevCommandTracker.DevModeActive();
    }





    //Externals





}
