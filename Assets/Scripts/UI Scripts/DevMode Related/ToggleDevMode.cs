using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDevMode : MonoBehaviour
{

    public void ToggleMode()
    {
        DevCommandTracker.ToggleDevMode();

        ToggleUiOnClick devUi = GetComponent<ToggleUiOnClick>();
    }
}
