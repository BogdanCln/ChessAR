using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static GameSettings Settings = new GameSettings(0,0);

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void SliderDiff(float value)
    {
        Settings.difficulty = (Difficulty)(value);
    }
    public void SliderColor(float value)
    {
        Settings.color = (PiecesColor)(value);
    }
}
