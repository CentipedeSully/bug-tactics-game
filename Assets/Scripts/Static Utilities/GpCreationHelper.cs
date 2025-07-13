using SullysToolkit.TableTop;
using UnityEngine;

public static class GpCreationHelper
{
    public static GpCreator _creator;




    public static void SetCreator(GpCreator creator) {  _creator = creator; }
    public static void RequestGpCreation(string name, GamePieceType type)
    {
        if (type != GamePieceType.Unset)
            _creator.CreateNewGamePiece(name, type);

        else Debug.LogWarning($"Ignoring the creation request of gamePiece {name} of unset type");
    }

}
