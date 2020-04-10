using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override List<Vector2Int> MoveLocations(Vector2Int gridPoint, Simulator simulator = null)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int dir in RookDirections)
        {
            for (int i = 1; i < 8; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                locations.Add(nextGridPoint);

                GameObject piece;
                if (simulator == null) piece = GameManager.instance.PieceAtGrid(nextGridPoint);
                else piece = simulator.PieceAtGrid(nextGridPoint);

                if (piece)
                {
                    break;
                }
            }
        }

        return locations;
    }
}
