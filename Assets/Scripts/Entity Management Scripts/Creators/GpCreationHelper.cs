using SullysToolkit.TableTop;
using UnityEngine;

public static class GpCreationHelper
{
    private static GpCreator _creator;
    private static BagOfHolding _bagOfHolding;



    public static void SetCreator(GpCreator creator) {  _creator = creator; }
    public static void SetBagOfHolding(BagOfHolding bag) {  _bagOfHolding = bag; }
    public static void CreateGamePiece(GamePieceType pieceType)
    {
        _creator.CreateNewGamePiece(pieceType);
    }

}
