using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint, Simulator simulator = null)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        int forwardDirection;
        if (simulator == null) forwardDirection = GameManager.instance.currentPlayer.forward;
        else forwardDirection = simulator.currentPlayer.forward;


        Vector2Int forwardOne = new Vector2Int(gridPoint.x, gridPoint.y + forwardDirection);
        Vector2Int forwardTwo = new Vector2Int(gridPoint.x, gridPoint.y + 2 * forwardDirection);
        Vector2Int forwardRight = new Vector2Int(gridPoint.x + 1, gridPoint.y + forwardDirection);
        Vector2Int forwardLeft = new Vector2Int(gridPoint.x - 1, gridPoint.y + forwardDirection);
        bool hasPawnMoved;

        GameObject pieceForwardOne, pieceForwardTwo, pieceForwardRight, pieceForwardLeft;
        if (simulator == null)
        {
            pieceForwardRight = GameManager.instance.PieceAtGrid(forwardRight);
            pieceForwardLeft = GameManager.instance.PieceAtGrid(forwardLeft);
            hasPawnMoved = GameManager.instance.HasPawnMoved(gameObject);
            pieceForwardOne = GameManager.instance.PieceAtGrid(forwardOne);
            pieceForwardTwo = GameManager.instance.PieceAtGrid(forwardTwo);
        }
        else
        {
            pieceForwardRight = simulator.PieceAtGrid(forwardRight);
            pieceForwardLeft = simulator.PieceAtGrid(forwardLeft);
            hasPawnMoved = simulator.HasPawnMoved(gameObject);
            pieceForwardOne = simulator.PieceAtGrid(forwardOne);
            pieceForwardTwo = simulator.PieceAtGrid(forwardTwo);
        }

        if (pieceForwardOne == false)
        {
            locations.Add(forwardOne);
        }

        

        if (hasPawnMoved == false && pieceForwardTwo == false && !pieceForwardOne)
        {
            locations.Add(forwardTwo);
        }

        if (pieceForwardRight)
        {
            locations.Add(forwardRight);
        }

        
        if (pieceForwardLeft)
        {
            locations.Add(forwardLeft);
        }

        return locations;
    }
}
