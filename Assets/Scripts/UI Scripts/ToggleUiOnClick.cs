using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUiOnClick : MonoBehaviour
{
    //Declarations
    [SerializeField] private GameObject _targetUi;




    //externals
    public void ToggleTargetUi()
    {
        if (_targetUi != null)
        {
            //Deactivate
            if (_targetUi.activeSelf)
            {
                _targetUi.SetActive(false);
                //_btn.spriteState = _btn.spriteState.
            }
                
            //Activate
            else
            {
                _targetUi.SetActive(true);
            }
        }
    }

    public bool IsTargetActive()
    {
        return _targetUi.activeSelf;
    }



}
