using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SullysToolkit
{
    //Interfaces
    public interface IMouseInteractable
    {
        void LeftClick();

        void RightClick();

        void MiddleClick();

        void OnHover();

        void OnHoverAfterDelay();

        void OnHoverExit();

    }


    public class MouseRaycaster : MonoBehaviour
    {
        //Declarations
        [Header("Raycaster Settings")]
        [SerializeField] private ReadInput _inputReader;
        [SerializeField] private string _RaycasterName = "Unnamed Selector";
        [SerializeField] private float _raycastDistance = 50;
        [SerializeField] private Vector3 _castDirection = Vector3.back;
        [SerializeField] private LayerMask _selectableLayers;
        [SerializeField] private MouseToWorld2D _mouseToWorld2DReference;

        //Debugging
        [Header("Debugging Utils")]
        [SerializeField] private bool _isDebugActive;
        [SerializeField] private Color _gizmoColor;


        //Monos
        private void Update()
        {
            RaycastFromMouse();
        }

        private void OnDrawGizmosSelected()
        {
            DrawMousePointer();
        }





        //Internal Utils
        private RaycastHit2D CastRayFromMouse()
        {
            Vector2 castOrigin = _mouseToWorld2DReference.GetWorldPosition();
            return Physics2D.Raycast(castOrigin, _castDirection,_raycastDistance,_selectableLayers);
        }



        //Getters, Setters, and Commands
        public string GetRaycasterName()
        {
            return _RaycasterName;
        }

        public LayerMask GetSelectableLayers()
        {
            return _selectableLayers;
        }


        public bool IsDebugActive()
        {
            return _isDebugActive;
        }

        public void SetDebugMode(bool newValue)
        {
            _isDebugActive = newValue;
        }


        //Debugging
        private void DrawMousePointer()
        {
            Gizmos.color = _gizmoColor;
            Vector3 mouseWorldPosition = _mouseToWorld2DReference.GetWorldPosition();
            Vector3 debugLineStart = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, Camera.main.transform.position.z);
            Vector3 debugLineEnd = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, _raycastDistance);
            Gizmos.DrawLine(debugLineStart, debugLineEnd);
        }

        private void RaycastFromMouse()
        {
            if (_inputReader != null)
            {
                if (_inputReader.LeftClick())
                {
                    RaycastHit2D hits = CastRayFromMouse();
                    if (hits.collider != null)
                    {
                        Debug.Log($"Clicked on '{hits.collider.name}'");
                    }
                    
                }
            }

        }

    }
}

