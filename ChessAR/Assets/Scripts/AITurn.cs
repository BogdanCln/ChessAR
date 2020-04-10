using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITurn : MachineState
{
    void Start()
    {
    }

    public void EnterState()
    {
        this.enabled = true;

        Simulator currentTable = new Simulator(GameManager.instance.pieces, GameManager.instance.movedPawns,
                                                GameManager.instance.currentPlayer, GameManager.instance.otherPlayer);

        GameObject piece = null;
        Vector2Int move = Vector2Int.zero;
        foreach (var pcs in currentTable.currentPlayer.pieces)
        {
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
