using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    // Sa nu lucrezi pe referintele de la pieces and movedPawns, ele sunt legate de tabla reala
    private GameObject[,] pieces;
    private List<GameObject> movedPawns;

    public Player currentPlayer;
    public Player otherPlayer;
    
    public Simulator(GameObject[,] pcs, List<GameObject> mvdPawns, Player current, Player other)
    {
        pieces = (GameObject[,]) pcs.Clone();
        movedPawns = new List<GameObject>(mvdPawns);
        currentPlayer = new Player(current);
        otherPlayer = new Player(other);
    }
    // Se poate duplica o tabla
    public Simulator(Simulator sim) {
        pieces = (GameObject[,]) sim.pieces.Clone();
        movedPawns = new List<GameObject>(sim.movedPawns);
        currentPlayer = sim.currentPlayer;
        otherPlayer = sim.otherPlayer;
    }
    // Aici sunt optiuniile de mutari pentru o piesa in contextul playerului curent si tablei curente
    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        // Posibila sursa de probleme
        List<Vector2Int> locations = piece.MoveLocations(gridPoint, this);

        // filter out offboard locations
        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        return locations;
    }
    // Asa se muta o piesa
    // Se poate observa daca a fost luata o piesa daca currentPlayer.capturedPieces.Count a crescut
    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Piece pieceComponent = piece.GetComponent<Piece>();
        if (pieceComponent.type == PieceType.Pawn && !HasPawnMoved(piece))
        {
            movedPawns.Add(piece);
        }

        // Nu era in GameManager
        if (pieces[gridPoint.x, gridPoint.y] != null)
        {
            CapturePieceAt(gridPoint);
        }

        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;
    }

    public void CapturePieceAt(Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        if (pieceToCapture.GetComponent<Piece>().type == PieceType.King)
        {
            Debug.Log(currentPlayer.name + " wins!");
        }
        // Nu era in GameManager
        otherPlayer.pieces.Remove(pieceToCapture);
        currentPlayer.capturedPieces.Add(pieceToCapture);
        pieces[gridPoint.x, gridPoint.y] = null;
    }

    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public bool HasPawnMoved(GameObject pawn)
    {
        return movedPawns.Contains(pawn);
    }

    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    // De apelat la final de mutare
    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
    }
}
