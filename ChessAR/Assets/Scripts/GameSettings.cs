using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    easy = 0,
    hard = 1
}

public enum PiecesColor
{
    black = 0,
    white = 1
}

public class GameSettings
{
    public Difficulty difficulty;
    public PiecesColor color;

    public GameSettings(PiecesColor col, Difficulty diff)
    {
        color = col;
        difficulty = diff;
    }
}

