using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int position { get; set; }

    void Update()
    {
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
        GameObject board = gameObject.transform.parent.gameObject;
        switch (touchPhase)
        {
            case TouchPhase.Began:
                RaycastHit hit;
                if (Physics.Raycast(touchRay, out hit))
                {
                    if (hit.collider == gameObject.GetComponent<Collider>())
                    {
                        if (board.GetComponent<TileSelector>().enabled) board.GetComponent<TileSelector>().onClick(position);
                        else if (board.GetComponent<MoveSelector>().enabled) board.GetComponent<MoveSelector>().onClick(position);
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
}
