using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int position { get; set; }

    // Update is called once per frame
    void Update()
    {
        GameObject board = gameObject.transform.parent.gameObject;
        if (Input.touchCount > 0)
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider == gameObject.GetComponent<Collider>())
                        {
                            if (board.GetComponent<TileSelector>().enabled) board.GetComponent<TileSelector>().onClick(position);
                            else if (board.GetComponent<MoveSelector>().enabled) board.GetComponent<MoveSelector>().onClick(position);
                        }
                    }
                }
            }
        }
    }
}
