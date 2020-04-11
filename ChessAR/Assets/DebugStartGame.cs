using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameSettings gameSettings = new GameSettings(PiecesColor.black, Difficulty.easy);
        GameManager.instance.SendMessage("StartGame", gameSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
