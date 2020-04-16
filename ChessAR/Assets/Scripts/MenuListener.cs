using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            FindInActiveObjectByName("CanvasMenu").SetActive(true);
            FindInActiveObjectByName("CanvasIntro").SetActive(false);
        }
    }

    GameObject FindInActiveObjectByName(string name)
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


    public void ResumeGame()
    {

        FindInActiveObjectByName("CanvasMenu").SetActive(false);
        FindInActiveObjectByName("CanvasIntro").SetActive(true);
    }
}

