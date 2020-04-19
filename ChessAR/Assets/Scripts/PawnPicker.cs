using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPicker : MonoBehaviour
{
    public delegate void callbackType(PieceType p);
    private static callbackType callback;

    public GameObject picker;
    private static GameObject _picker;

    void Start()
    {
        /*
        GameObject newPiece = Instantiate(picker);
        newPiece.transform.parent = gameObject.transform;
        newPiece.transform.localScale = new Vector3(1.666f, 1.666f, 1.666f);
        newPiece.transform.localPosition = new Vector3(0.0f, 6.0f, 0.0f);
        //newPiece.transform.localRotation = gameObject.transform.rotation;
        _picker = newPiece;
        */
    }

    public static void Prepare(GameObject parent)
    {

        /*GameObject newPiece = Instantiate(MenuListener.FindInActiveObjectByName("PawnPicker"));
        newPiece.transform.parent = parent.transform;
        newPiece.transform.localScale = new Vector3(1.666f, 1.666f, 1.666f);
        newPiece.transform.localPosition = new Vector3(0.0f, 6.0f, 0.0f);
        //newPiece.transform.localRotation = gameObject.transform.rotation;*/
        
    }

    public static void Activate(callbackType _callback)
    {
        callback = _callback;
        _picker = MenuListener.FindInActiveObjectByName("PawnPicker");
        _picker.SetActive(true);
        //Instantiate(MenuListener.FindInActiveObjectByName("PawnPicker"));
        //MenuListener.FindInActiveObjectByName("PawnPicker").SetActive(true);
        //GameManager.instance.board_picker.SetActive(true);
        /*
        foreach (Transform child in GameManager.instance.board.transform)
        {
            if (child.name == "PawnPicker")
            {
                //MenuListener.FindInActiveObjectByName("DDDText").SetActive(true);
                child.gameObject.SetActive(true);
                Debug.Log("Child found. Mame: " + child.name);
                break;
            }
        }*/


    }

    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 15);

        // Handle native touch events
        foreach (Touch touch in Input.touches)
        {
            HandleTouch(Camera.main.ScreenPointToRay(touch.position), touch.phase);
        }

        // Simulate touch events from mouse events
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch(Camera.main.ScreenPointToRay(Input.mousePosition), TouchPhase.Began);
            }
            if (Input.GetMouseButton(0))
            {
                HandleTouch(Camera.main.ScreenPointToRay(Input.mousePosition), TouchPhase.Moved);
            }
            if (Input.GetMouseButtonUp(0))
            {
                HandleTouch(Camera.main.ScreenPointToRay(Input.mousePosition), TouchPhase.Ended);
            }
        }
    }

    private void HandleTouch(Ray touchRay, TouchPhase touchPhase)
    {
        
        switch (touchPhase)
        {
            case TouchPhase.Began:
                RaycastHit hit;
                if (Physics.Raycast(touchRay, out hit))
                {
                    if (hit.collider == MenuListener.FindInActiveObjectByName("PPKnight").GetComponent<Collider>())
                    {
                        HandlePick(PieceType.Knight);
                    }
                    else if (hit.collider == MenuListener.FindInActiveObjectByName("PPBishop").GetComponent<Collider>())
                    {
                        HandlePick(PieceType.Bishop);
                    }
                    else if (hit.collider == MenuListener.FindInActiveObjectByName("PPRook").GetComponent<Collider>())
                    {
                        HandlePick(PieceType.Rook);
                    }
                    else if (hit.collider == MenuListener.FindInActiveObjectByName("PPQueen").GetComponent<Collider>())
                    {
                        HandlePick(PieceType.Queen);
                    }
                }
                break;
            case TouchPhase.Moved:
                // Not needed
                break;
            case TouchPhase.Ended:
                // Not needed
                break;
        }
    }
    private void HandlePick(PieceType choice)
    {
        // UI
        _picker.SetActive(false);

        // TO SEND CHOICE TO FUNCTIONAL PARTS
        callback(choice);
    }
    private void DebugChoice(PieceType choice)
    {
        Debug.Log("Choice: " + choice);
    }
}
