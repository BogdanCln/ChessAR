using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : MachineState
{
    void Start() { }
    public static int GetPieceEvaluation(PieceType type)
    {
        switch (type)
        {
            case PieceType.Queen: return -900;
            case PieceType.King: return -90;
            case PieceType.Rook: return -50;
            case PieceType.Knight: return -30;
            case PieceType.Bishop: return -30;
            case PieceType.Pawn: return -10;
            default: return 0;
        }
    }
    public void EnterState()
    {
        enabled = true;

        Simulator currentTable = new Simulator(GameManager.instance.pieces, GameManager.instance.movedPawns,
                                                GameManager.instance.currentPlayer, GameManager.instance.otherPlayer);

        GameObject piece = null;
        Vector2Int move = Vector2Int.zero;
        foreach (GameObject pcs in currentTable.currentPlayer.pieces)
        {
            // Piece pieceComponent = pcs.GetComponent<Piece>();
            // Debug.Log(pieceComponent.type);
            // Debug.Log(GetPieceEvaluation(pieceComponent.type));

            List<Vector2Int> moves = currentTable.MovesForPiece(pcs);
            if (moves.Count > 0)
            {
                piece = pcs;
                move = moves[0];
            }
        }
        GameManager.instance.Move(piece, move);

        // Se termina functia in ExistState pentru a continua jocul
        ExitState();
    }

    private void ExitState()
    {
        if (enabled)
        {
            enabled = false;
            GameManager.instance.NextPlayer();
            GetComponent<PlayerRoutineSelection>().EnterState();
        }
    }

    public override void CancelState()
    {
        enabled = false;
    }
}
