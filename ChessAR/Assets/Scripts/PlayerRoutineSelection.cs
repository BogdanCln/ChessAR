using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoutineSelection : MachineState
{
    void Start()
    {
    }

    public void EnterState()
    {
        enabled = true;

        bool pat = true;

        foreach(var piece in GameManager.instance.currentPlayer.pieces)
        {
            if (GameManager.instance.MovesForPiece(piece).Count > 0)
            {
                pat = false;
                break;
            }
        }
        if (pat) GameManager.instance.DeclarePat();

        ExitState(GameManager.instance.currentPlayer.AI);
    }

    private void ExitState(bool AI)
    {
        enabled = false;
        if (AI) GetComponent<AITurn>().EnterState();
        else GetComponent<TileSelector>().EnterState();
    }

    public override void CancelState()
    {
        this.enabled = false;
    }
}
