using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
        if (Input.GetKey(KeyCode.Escape) && timer > 0.5) {
            timer = 0;
            ToggleGameMenu();
        }
        UpdateMessage();
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

    static private int _mframes = 0;
    static private int _mframesActive = 0;
    static private float _mframesInterval = 0.1f;
    static private float _malpha = 0;
    static private TMP_Text _txt;
    private void UpdateMessage()
    {
        if (_mframes <= 0) return;
        _mframes--;
        if (_mframesActive > 0)
        {
            _mframesActive--;
            return;
        }
        _malpha -= _mframesInterval;
        _txt.alpha = _malpha;
        //_txt.text = _malpha.ToString();
    }
    public static void ShowMessage(string message, int frames){
        _txt = FindInActiveObjectByName("ShownText").GetComponent<TMP_Text>();
        _mframes = frames;
        _mframesActive = frames / 9;
        _malpha = 1;
        _mframesInterval = 1.0f / (_mframes - _mframesActive);
        _txt.text = message;
        _txt.alpha = 1;
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
        MenuListener.ShowMessage("Mesaj",120);
        //var gameObject = Instantiate(obj);
        //PawnPicker.Prepare(gameObject); // setez pawn picker

        //PawnPicker.Activate(DebugChoice);
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

