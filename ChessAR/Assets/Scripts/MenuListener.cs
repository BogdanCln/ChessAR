using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuListener : MonoBehaviour
{
    private double timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.Escape) && timer>0.5) {
            timer = 0;
            ToggleGameMenu();
        }
    }

    public void RestartGame()
    {
        GameManager.instance.RestartGame(GameManager.instance.settings);
        ToggleGameMenu();
        /*
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        */
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ToggleGameMenu()
    {
        bool on = !FindInActiveObjectByName("CanvasMenu").activeSelf;
        FindInActiveObjectByName("CanvasMenu").SetActive(on);
        try
        {
            FindInActiveObjectByName("CanvasIntro").SetActive(!on);
        }
        catch { }
    }

    public static GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
    public void DebugButton(GameObject obj)
    {

        //var gameObject = Instantiate(obj);
        //PawnPicker.Prepare(gameObject); // setez pawn picker
        
        PawnPicker.Activate(DebugChoice);
    }
    private void DebugChoice(PieceType choice)
    {
        MenuListener.FindInActiveObjectByName("DDDText").SetActive(true);
        Debug.Log("Choice: " + choice);
    }
    public void ResumeGame()
    {
        ToggleGameMenu();
    }
}

