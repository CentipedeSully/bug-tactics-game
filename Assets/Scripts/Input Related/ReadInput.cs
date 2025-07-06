using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadInput : MonoBehaviour
{
    [SerializeField] private bool _lClick;
    [SerializeField] private bool _rClick;
    [SerializeField] private Vector2 _directionalInput;

    

    public void ReadLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed )
        {
            _lClick = true;
            Debug.Log("LClick");
        }
        else _lClick = false;
    }

    public void ReadRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _rClick = true;
            Debug.Log("RClick");
        }
        else _rClick = false;
    }


    public void ReadDirectionalInput(InputAction.CallbackContext context)
    {
        _directionalInput = context.ReadValue<Vector2>();
    }


    public bool LeftClick() { return _lClick; }
    public bool RightClick() { return _rClick; }
    public Vector2 DirectionalInput() { return _directionalInput; }
}
