using UnityEngine;

public class Geometry
{
    static public Vector3 PointFromGrid(Vector2Int gridPoint)
    {
        /*        float x = -.1535f + .045f * gridPoint.x;
                float z = -.1535f + .045f * gridPoint.y;*/
        float x = -5.25f + 1.5f * gridPoint.x;
        float z = -5.25f + 1.5f * gridPoint.y;
        return new Vector3(x, .1f, z);
    }

    static public Vector2Int GridPoint(int col, int row)
    {
        return new Vector2Int(col, row);
    }

/*    static public Vector2Int GridFromPoint(Vector3 point)
    {
        int col = Mathf.FloorToInt(.18f + 19.5f * point.x);
        int row = Mathf.FloorToInt(.18f + 19.5f * point.z);
        return new Vector2Int(col, row);
    }*/
}
