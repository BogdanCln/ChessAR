using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    

    public void RestartGame()
    {
        GameManager.instance.RestartGame(GameManager.instance.settings);

        /*
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        */
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
}
