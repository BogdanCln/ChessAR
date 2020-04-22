using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingChoiceState : MachineState
{
    GameObject pawn;
    Vector2Int position;

    public void EnterState(GameObject pawn, Vector2Int position)
    {
        this.pawn = pawn;
        this.position = position;
        this.enabled = true;

        PawnPicker.Activate(OnChoice);
    }

    public void OnChoice (PieceType type)
    {
        GameManager.instance.currentPlayer.pieces.Remove(pawn);
        GameManager.instance.movedPawns.Remove(pawn);
        Destroy(pawn);
        GameManager.instance.pieces[position.x, position.y] = null;

        GameObject prefab;
        switch(type)
        {
            case PieceType.Queen:
                if (GameManager.instance.currentPlayer.name == "black")
                    prefab = GameManager.instance.blackQueen;
                else
                    prefab = GameManager.instance.whiteQueen;
                break;
            case PieceType.Bishop:
                if (GameManager.instance.currentPlayer.name == "black")
                    prefab = GameManager.instance.blackBishop;
                else
                    prefab = GameManager.instance.whiteBishop;
                break;
            case PieceType.Rook:
                if (GameManager.instance.currentPlayer.name == "black")
                    prefab = GameManager.instance.blackRook;
                else
                    prefab = GameManager.instance.whiteRook;
                break;
            default:
                if (GameManager.instance.currentPlayer.name == "black")
                    prefab = GameManager.instance.blackKnight;
                else
                    prefab = GameManager.instance.whiteKnight;
                break;
        }

        GameManager.instance.AddPiece(prefab, GameManager.instance.currentPlayer, position.x, position.y);
        ExitState();
    }

    public void ExitState()
    {
        this.enabled = false;
        GameManager.instance.NextPlayer();
        GetComponent<PlayerRoutineSelection>().EnterState();
    }

    public override void CancelState()
    {
        this.enabled = false;
    }
}
