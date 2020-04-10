using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MachineState
{
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;
    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

    void Start()
    {
        tileHighlight = Instantiate(tileHighlightPrefab);
        tileHighlight.transform.parent = gameObject.transform;
        tileHighlight.transform.localScale = .14f * Vector3.one;
        tileHighlight.transform.localPosition = Geometry.PointFromGrid(new Vector2Int(0, 0));
        tileHighlight.SetActive(false);
    }

    /*    void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                Vector2Int gridPoint = Geometry.GridFromPoint(point);

                tileHighlight.SetActive(true);
                tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
                if (Input.GetMouseButtonDown(0))
                {
                    // Reference Point 2: check for valid move location
                    if (!moveLocations.Contains(gridPoint))
                    {
                        return;
                    }

                    if (GameManager.instance.PieceAtGrid(gridPoint) == null)
                    {
                        GameManager.instance.Move(movingPiece, gridPoint);
                    }
                    else
                    {
                        GameManager.instance.CapturePieceAt(gridPoint);
                        GameManager.instance.Move(movingPiece, gridPoint);
                    }
                    // Reference Point 3: capture enemy piece here later
                    ExitState();
                }
            }
            else
            {
                tileHighlight.SetActive(false);
            }
        }
    */

    public void onClick(Vector2Int gridPoint)
    {
        tileHighlight.SetActive(true);
        tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
        if (!moveLocations.Contains(gridPoint))
        {
            CancelMove();
            return;
        }

        GameManager.instance.Move(movingPiece, gridPoint);

        ExitState();
    }

    private void CancelMove()
    {
        this.enabled = false;

        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

        GameManager.instance.DeselectPiece(movingPiece);
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
    }

    public void EnterState(GameObject piece)
    {
        movingPiece = piece;
        this.enabled = true;

        moveLocations = GameManager.instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

        if (moveLocations.Count == 0)
        {
            CancelMove();
        }

        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (GameManager.instance.PieceAtGrid(loc))
            {
                highlight = Instantiate(attackLocationPrefab);
            }
            else
            {
                highlight = Instantiate(moveLocationPrefab);
            }
            highlight.transform.parent = gameObject.transform;
            highlight.transform.localScale = .14f * Vector3.one;
            highlight.transform.localRotation = gameObject.transform.localRotation;
            highlight.transform.localPosition = Geometry.PointFromGrid(loc);
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState()
    {
        if (enabled)
        {
            enabled = false;
            tileHighlight.SetActive(false);
            GameManager.instance.DeselectPiece(movingPiece);
            movingPiece = null;
            GameManager.instance.NextPlayer();
            foreach (GameObject highlight in locationHighlights)
            {
                Destroy(highlight);
            }
            PlayerRoutineSelection routine = GetComponent<PlayerRoutineSelection>();
            routine.EnterState();
        }
    }

    public override void CancelState()
    {
        enabled = false;
        tileHighlight.SetActive(false);
        movingPiece = null;
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
