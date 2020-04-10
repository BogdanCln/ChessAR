using UnityEngine;

public class Board : MonoBehaviour
{
    public Material defaultMaterial;
    public Material selectedMaterial;

    public GameObject AddPiece(GameObject piece, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row);
        GameObject newPiece = Instantiate(piece);
        newPiece.transform.parent = gameObject.transform;
        newPiece.transform.localScale = Vector3.one;
        newPiece.transform.localPosition = Geometry.PointFromGrid(gridPoint);
        newPiece.transform.localRotation = gameObject.transform.rotation;
        return newPiece;
    }

    public GameObject AddTile(GameObject prefab, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        GameObject tile = Instantiate(prefab);
        tile.transform.parent = gameObject.transform;
        tile.transform.localScale = .14f * Vector3.one;
        tile.transform.localRotation = gameObject.transform.localRotation;
        tile.transform.localPosition = point;

        tile.GetComponent<Tile>().position = new Vector2Int(col, row);
        return tile;
    }

    public void RemovePiece(GameObject piece)
    {
        Destroy(piece);
    }

    public void MovePiece(GameObject piece, Vector2Int gridPoint)
    {
        piece.transform.localPosition = Geometry.PointFromGrid(gridPoint);
    }

    public void SelectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = selectedMaterial;
    }

    public void DeselectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = defaultMaterial;
    }
}
