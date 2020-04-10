using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MachineState
{
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;

    void Start()
    {
        Vector2Int gridPoint = Geometry.GridPoint(4, 4);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        tileHighlight = Instantiate(tileHighlightPrefab);
        tileHighlight.transform.parent = gameObject.transform;
        tileHighlight.transform.localScale = .14f * Vector3.one;
        tileHighlight.transform.localRotation = gameObject.transform.localRotation;
        tileHighlight.transform.localPosition = point;
        tileHighlight.SetActive(false);

        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        /*if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began) 
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 point = hit.point;
                        point = gameObject.transform.position - point;
                        point = gameObject.transform.parent.parent.rotation * point;
                        Vector2Int gridPoint = Geometry.GridFromPoint(point);
                        Debug.Log(point.ToString("F4"));
                        // Debug.Log(gameObject.transform.rotation.ToString("F4"));
                        
                        tileHighlight.SetActive(true);
                        tileHighlight.transform.localPosition = Geometry.PointFromGrid(gridPoint);
                        if (Input.GetMouseButtonDown(0))
                        {
                            GameObject selectedPiece = GameManager.instance.PieceAtGrid(gridPoint);
                            if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                            {
                                GameManager.instance.SelectPiece(selectedPiece);
                                // ExitState(selectedPiece);
                            }
                        }
                    }
                    else
                    {
                        tileHighlight.SetActive(false);
                    }
                }
            }
        }*/
    }

    public void onClick(Vector2Int point)
    {
        tileHighlight.SetActive(true);
        tileHighlight.transform.localPosition = Geometry.PointFromGrid(point);

        GameObject selectedPiece = GameManager.instance.PieceAtGrid(point);
        if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
        {
            GameManager.instance.SelectPiece(selectedPiece);
            ExitState(selectedPiece);
        }
    }

    public void EnterState()
    {
        this.enabled = true;
    }

    private void ExitState(GameObject movingPiece)
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        MoveSelector move = GetComponent<MoveSelector>();
        move.EnterState(movingPiece);
    }

    public override void CancelState()
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
    }
}


