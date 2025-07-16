using SullysToolkit.TableTop;
using UnityEngine;

public static class GpCreationHelper
{
    public static GpCreator _creator;




    public static void SetCreator(GpCreator creator) {  _creator = creator; }
    public static void CreateGamePiece(GamePieceType pieceType, (int,int) position)
    {
        //_creator.CreateNewGamePiece()
    }

}
