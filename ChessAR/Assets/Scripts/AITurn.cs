using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEvaluation
{
    public int value;
    public GameObject piece;
    public Vector2Int move;

    public AIEvaluation(int e)
    {
        value = e;
    }
    public AIEvaluation(int e, GameObject p, Vector2Int m)
    {
        value = e;
        piece = p;
        move = m;
    }
    static public AIEvaluation Min(AIEvaluation e1, AIEvaluation e2)
    {
        return (e1.value < e2.value) ? e1 : e2;
    }
    static public AIEvaluation Max(AIEvaluation e1, AIEvaluation e2)
    {
        return (e1.value > e2.value) ? e1 : e2;
    }
}

public class AITurn : MachineState
{
    void Start() { }
    public static int GetPieceEvaluation(PieceType type)
    {
        switch (type)
        {
            case PieceType.Queen: return 90;
            case PieceType.King: return 9;
            case PieceType.Rook: return 5;
            case PieceType.Knight: return 3;
            case PieceType.Bishop: return 3;
            case PieceType.Pawn: return 1;
            default: return 0;
        }
    }

    private AIEvaluation MiniMax(Simulator table, int depth, GameObject parent, Vector2Int parentMove)
    {
        if (depth == 0)
        {
            return new AIEvaluation(TableEvaluation(table), parent, parentMove);
        }

        if (table.currentPlayer.AI)
        {
            // Maximizer is the Bot
            AIEvaluation evaluation = new AIEvaluation(-10000);

            foreach (GameObject piece in table.currentPlayer.pieces)
            {
                foreach (var move in table.MovesForPiece(piece))
                {
                    Simulator childTable = new Simulator(table.pieces, table.movedPawns, table.currentPlayer, table.otherPlayer);

                    childTable.Move(piece, move);
                    childTable.NextPlayer();

                    evaluation = AIEvaluation.Max(evaluation, MiniMax(childTable, depth - 1, piece, move));
                }
            }

            return evaluation;
        }
        else
        {
            // Minimizer is the Human
            AIEvaluation evaluation = new AIEvaluation(10000);

            foreach (GameObject piece in table.currentPlayer.pieces)
            {
                foreach (var move in table.MovesForPiece(piece))
                {
                    Simulator childTable = new Simulator(table.pieces, table.movedPawns, table.currentPlayer, table.otherPlayer);

                    childTable.Move(piece, move);
                    childTable.NextPlayer();

                    evaluation = AIEvaluation.Min(evaluation, MiniMax(childTable, depth - 1, piece, move));
                }
            }

            return evaluation;
        }
    }


    private int TableEvaluation(Simulator table)
    {
        int evaluation = 0;
        foreach (GameObject piece in table.currentPlayer.pieces)
        {
            Piece pieceComponent = piece.GetComponent<Piece>();
            if (table.currentPlayer.AI)
                evaluation += GetPieceEvaluation(pieceComponent.type);
            else
                evaluation -= GetPieceEvaluation(pieceComponent.type);
        }
        foreach (GameObject piece in table.otherPlayer.pieces)
        {
            Piece pieceComponent = piece.GetComponent<Piece>();
            if (table.otherPlayer.AI)
                evaluation += GetPieceEvaluation(pieceComponent.type);
            else
                evaluation -= GetPieceEvaluation(pieceComponent.type);
        }

        return evaluation;
    }

    public void EnterState()
    {
        enabled = true;

        Simulator currentTable = new Simulator(GameManager.instance.pieces, GameManager.instance.movedPawns,
                                                GameManager.instance.currentPlayer, GameManager.instance.otherPlayer);

        AIEvaluation MiniMaxEval = MiniMax(currentTable, 3, null, Vector2Int.zero);

        Debug.Log("MiniMaxEval: " + MiniMaxEval.value);

        GameManager.instance.Move(MiniMaxEval.piece, MiniMaxEval.move);

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
