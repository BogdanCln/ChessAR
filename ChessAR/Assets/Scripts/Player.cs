using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public List<GameObject> pieces;
    public List<GameObject> capturedPieces;

    public string name;
    public int forward;
    public bool AI;

    public Player(string name, bool positiveZMovement, bool AI = false)
    {
        this.name = name;
        this.AI = AI;
        pieces = new List<GameObject>();
        capturedPieces = new List<GameObject>();

        if (positiveZMovement == true)
        {
            this.forward = 1;
        }
        else
        {
            this.forward = -1;
        }
    }

    public Player(Player p)
    {
        name = p.name;
        forward = p.forward;
        AI = p.AI;

        pieces = new List<GameObject>(p.pieces);
        capturedPieces = new List<GameObject>(p.capturedPieces);
    }
}
