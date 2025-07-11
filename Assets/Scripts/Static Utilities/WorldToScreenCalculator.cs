using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldToScreenCalculator
{
    private static Camera _activeCamera;



    public static void SetCamera(Camera activeCamera) {  _activeCamera = activeCamera; }
    public static Vector2 GetScreenPosition(Vector3 worldPosition)
    {
        return _activeCamera.WorldToScreenPoint(worldPosition);
    }
}
