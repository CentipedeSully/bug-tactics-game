using SullysToolkit.TableTop;
using UnityEngine;

public static class GpCreationHelper
{

    public delegate void GpCreationHelperEvent(string name, GamePieceType type);
    public static event GpCreationHelperEvent OnCreationRequested;

    


    public static void RequestGpCreation(string name, GamePieceType type)
    {
        if (type != GamePieceType.Unset)
            OnCreationRequested?.Invoke(name, type);

        else Debug.LogWarning($"Ignoring the creation request of gamePiece {name} of unset type");
    }

}
