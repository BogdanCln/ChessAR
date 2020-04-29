using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;
    public GameSettings settings;

    public GameObject whiteKing;
    public GameObject whiteQueen;
    public GameObject whiteBishop;
    public GameObject whiteKnight;
    public GameObject whiteRook;
    public GameObject whitePawn;

    public GameObject blackKing;
    public GameObject blackQueen;
    public GameObject blackBishop;
    public GameObject blackKnight;
    public GameObject blackRook;
    public GameObject blackPawn;

    public GameObject tilePrefab;

    public GameObject[,] pieces;
    public List<GameObject> movedPawns;
    public List<GameObject> movedPieces;

    private Player white;
    private Player black;
    public Player currentPlayer;
    public Player otherPlayer;

    private bool underCheck;

    void Awake()
    {
        instance = this;
    }

    public void StartGame(GameSettings settings)
    {
        this.settings = settings;
        this.underCheck = false;

        if (settings.color == PiecesColor.white)
        {
            white = new Player("white", true, false);
            black = new Player("black", false, true);
        }
        else
        {
            board.transform.Rotate(Vector3.up, 180.0f);
            white = new Player("white", true, true);
            black = new Player("black", false, false);
        }
        

        pieces = new GameObject[8, 8];
        movedPawns = new List<GameObject>();
        movedPieces = new List<GameObject>();

        currentPlayer = white;
        otherPlayer = black;

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                board.AddTile(tilePrefab, i, j);

        AddPiece(whiteRook, white, 0, 0);
        AddPiece(whiteKnight, white, 1, 0);
        AddPiece(whiteBishop, white, 2, 0);
        AddPiece(whiteQueen, white, 3, 0);
        AddPiece(whiteKing, white, 4, 0);
        AddPiece(whiteBishop, white, 5, 0);
        AddPiece(whiteKnight, white, 6, 0);
        AddPiece(whiteRook, white, 7, 0);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(whitePawn, white, i, 1);
        }

        AddPiece(blackRook, black, 0, 7);
        AddPiece(blackKnight, black, 1, 7);
        AddPiece(blackBishop, black, 2, 7);
        AddPiece(blackQueen, black, 3, 7);
        AddPiece(blackKing, black, 4, 7);
        AddPiece(blackBishop, black, 5, 7);
        AddPiece(blackKnight, black, 6, 7);
        AddPiece(blackRook, black, 7, 7);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(blackPawn, black, i, 6);
        }

        board.GetComponent<PlayerRoutineSelection>().EnterState();
    }

    public void EndGame()
    {
        if (settings.color == PiecesColor.black) board.transform.Rotate(Vector3.up, 180.0f);

        MachineState[] states = {board.GetComponent<PlayerRoutineSelection>(), board.GetComponent<AITurn>(),
                                board.GetComponent<TileSelector>(), board.GetComponent<MoveSelector>()};
        foreach (var state in states)
        {
            state.CancelState();
        }

        foreach (var piece in pieces)
        {
            if (piece != null)
                Destroy(piece);
        }
    }

    public void RestartGame(GameSettings settings)
    {
        EndGame();
        StartGame(settings);
    }

    public void AddPiece(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPiece(prefab, col, row);
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
        // Fixes the bug with knight orientation
        if (white.AI)
        {
            pieceObject.transform.localRotation *= Quaternion.Euler(0, 180f, 0);
        }
    }

    public void SelectPieceAtGrid(Vector2Int gridPoint)
    {
        GameObject selectedPiece = pieces[gridPoint.x, gridPoint.y];
        if (selectedPiece)
        {
            board.SelectPiece(selectedPiece);
        }
    }

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        List<Vector2Int> locations = piece.MoveLocations(gridPoint);

        // filter out offboard locations
        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);

        // filter out locations with friendly piece
        locations.RemoveAll(gp => FriendlyPieceAt(gp));
        
        // Smecheria aia cu rocada
        if (piece.type == PieceType.King && !movedPieces.Contains(pieceObject))
        {
            // Verifica rocada mica
            if (gridPoint.x + 3 < 8 && pieces[gridPoint.x + 3, gridPoint.y] != null && !movedPieces.Contains(pieces[gridPoint.x + 3, gridPoint.y]) &&
                    pieces[gridPoint.x + 1, gridPoint.y] == null && pieces[gridPoint.x + 2, gridPoint.y] == null)
                locations.Add(new Vector2Int(gridPoint.x + 2, gridPoint.y));
            // Verifica rocada mare
            if (gridPoint.x - 4 >= 0 && pieces[gridPoint.x - 4, gridPoint.y] != null && !movedPieces.Contains(pieces[gridPoint.x - 4, gridPoint.y]) && 
                    pieces[gridPoint.x - 1, gridPoint.y] == null && pieces[gridPoint.x - 2, gridPoint.y] == null && pieces[gridPoint.x - 3, gridPoint.y] == null)
                locations.Add(new Vector2Int(gridPoint.x - 3, gridPoint.y));
        }

        // filter check locations
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var loc in locations)
        {
            Simulator table = new Simulator(GameManager.instance.pieces, GameManager.instance.movedPawns,
                                            GameManager.instance.currentPlayer, GameManager.instance.otherPlayer);
            table.Move(pieceObject, loc);
            table.NextPlayer();

            Vector2Int kingPos = new Vector2Int(-1, -1);
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (table.pieces[i, j] != null && table.pieces[i, j].GetComponent<Piece>().type == PieceType.King)
                    {
                        if (table.otherPlayer.pieces.Contains(table.pieces[i, j]))
                        {
                            kingPos = new Vector2Int(i, j);
                            Debug.Log(kingPos);
                        }
                    }
                }

            if (kingPos == new Vector2Int(-1, -1))
            {
                Debug.LogWarning("Can't find the king of current player when computing moves! Can't check for check");
            }

            foreach (var pcs in table.currentPlayer.pieces)
            {
                List<Vector2Int> moves = table.MovesForPiece(pcs);
                if (moves.Contains(kingPos))
                {
                    toRemove.Add(loc);
                }
            }
        }
        foreach (var loc in toRemove)
        {
            locations.Remove(loc);
        }

        return locations;
    }

    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Piece pieceComponent = piece.GetComponent<Piece>();
        if (pieceComponent.type == PieceType.Pawn && !HasPawnMoved(piece))
        {
            movedPawns.Add(piece);
        }

        if (PieceAtGrid(gridPoint) != null) CapturePieceAt(gridPoint);

        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;
        board.MovePiece(piece, gridPoint);
        underCheck = false;

        // Smecheria aia cu rocada
        if (!movedPieces.Contains(piece)) movedPieces.Add(piece);
        if (pieceComponent.type == PieceType.King && Mathf.Abs(startGridPoint.x - gridPoint.x) > 1)
        {
            Vector2Int rook_start, rook_destination;
            // Verifica rocada mica
            if (gridPoint.x == 6)
            {
                rook_start = new Vector2Int(7, gridPoint.y);
                rook_destination = new Vector2Int(5, gridPoint.y);
            }
            // Rocada mare
            else
            {
                rook_start = new Vector2Int(0, gridPoint.y);
                rook_destination = new Vector2Int(2, gridPoint.y);
            }
            pieces[rook_destination.x, rook_destination.y] = pieces[rook_start.x, rook_start.y];
            board.MovePiece(pieces[rook_start.x, rook_start.y], rook_destination);
            pieces[rook_start.x, rook_start.y] = null;
        }

        // Alegere piesa noua
        if (pieceComponent.type == PieceType.Pawn && (gridPoint.y == 0 || gridPoint.y == 7))
        {
            if (!currentPlayer.AI)
            {
                board.GetComponent<MoveSelector>().TriggerPawnSelection(piece, gridPoint);
                MenuListener.ShowMessage("Chose a piece to replace your pawn", 120);
            }
            else
            {
                currentPlayer.pieces.Remove(piece);
                movedPawns.Remove(piece);
                Destroy(piece);
                pieces[gridPoint.x, gridPoint.y] = null;

                GameObject prefab;
                if (currentPlayer.name == "white") prefab = whiteQueen;
                else prefab = blackQueen;
                GameManager.instance.AddPiece(prefab, GameManager.instance.currentPlayer, gridPoint.x, gridPoint.y);
            }
        }
    }

    public void PawnMoved(GameObject pawn)
    {
        movedPawns.Add(pawn);
    }

    public bool HasPawnMoved(GameObject pawn)
    {
        return movedPawns.Contains(pawn);
    }

    public void CapturePieceAt(Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        otherPlayer.pieces.Remove(pieceToCapture);
        currentPlayer.capturedPieces.Add(pieceToCapture);
        pieces[gridPoint.x, gridPoint.y] = null;
        Destroy(pieceToCapture);

        if (pieceToCapture.GetComponent<Piece>().type == PieceType.King)
        {
            DeclareWin(currentPlayer);
        }
    }

    public void SelectPiece(GameObject piece)
    {
        board.SelectPiece(piece);
    }

    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
    }

    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }

    public void NextPlayer()
    {
        Vector2Int kingPos = new Vector2Int(-1, -1);
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] != null && pieces[i, j].GetComponent<Piece>().type == PieceType.King)
                {
                    if (otherPlayer.pieces.Contains(pieces[i, j]))
                    {
                        kingPos = new Vector2Int(i, j);
                    }
                }
            }

        if (kingPos == new Vector2Int(-1, -1))
        {
            Debug.LogWarning("Can't find the king of current player! Can't undeCheck for check");
            Player tmp = currentPlayer;
            currentPlayer = otherPlayer;
            otherPlayer = tmp;
            return;
        }

        foreach (var piece in currentPlayer.pieces)
        {
            List<Vector2Int> moves = MovesForPiece(piece);
            if (moves.Contains(kingPos))
            {
                underCheck = true;
                if (currentPlayer.AI)
                    MenuListener.ShowMessage("Check!", 120);
                break;
            }
        }

        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;

        bool pat = true;
        foreach (var piece in currentPlayer.pieces)
        {
            List<Vector2Int> moves = MovesForPiece(piece);
            if (moves.Count > 0)
            {
                pat = false;
                break;
            }
        }

        if (pat)
        {
            if (underCheck) DeclareWin(otherPlayer);
            else GameManager.instance.DeclarePat();
        }
    }

    public void DeclareWin(Player p)
    {
        Debug.Log(p.name + " wins!");
        //MenuListener.ShowMessage(p.name + " wins!", 120);
        // COMPLETARE AICI CU UI
        //RestartGame(settings);
        if (p.AI)
            MenuListener.FindInActiveObjectByName("CanvasLose").SetActive(true);
        else
            MenuListener.FindInActiveObjectByName("CanvasWin").SetActive(true);
    }

    public void DeclarePat()
    {
        // COMPLETARE CU UI
        Debug.Log("Pat!");
        MenuListener.ShowMessage("Draw!", 120);
        RestartGame(settings);
    }
}
