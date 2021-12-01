using System;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour {
    public GameObject PrefabCell;
    public Sprite CellAternate;
    [Min(0)] public int Width = 8, Height = 8;
    public float CellSize = 1.28f;
    public Transform BoardStart;

    public Action<BoardCell> OnBoardCellClicked;
    private BoardCell[,] _grid;
    private List<BoardCell> cellsHighlighted = new List<BoardCell>();

    private void Awake() => _grid = new BoardCell[Width, Height];

    public void Initialize() {
        for(int y = 0; y < Height; y++) {
            for(int x = 0; x < Width; x++) {
                Vector3 position = GetWorldPosition(x, y);
                GameObject cellGo = GameObject.Instantiate(PrefabCell, position, Quaternion.identity);
                BoardCell cell = cellGo.GetComponent<BoardCell>();
                cell.Position = new Vector2Int(x, y);
                _grid[x, y] = cell;
                cell.OnClick += OnCellClicked;

                if((x + y) % 2 == 1)
                    cellGo.GetComponent<SpriteRenderer>().sprite = CellAternate;

                cellGo.transform.SetParent(transform);
            }
        }
    }

    private void OnCellClicked(BoardCell cell) => OnBoardCellClicked?.Invoke(cell);


    public BoardCell GetCell(int xGrid, int yGrid) {
        if(xGrid < 0 || xGrid >= Width || yGrid < 0 || yGrid >= Height) return null;
        else return _grid[xGrid, yGrid];
    }

    public BoardCell GetCell(Vector2Int gridPosition) => GetCell(gridPosition.x, gridPosition.y);

    public Vector3 GetWorldPosition(int xGrid, int yGrid) {
        return new Vector3(xGrid * CellSize + BoardStart.transform.position.x,
                           yGrid * CellSize + BoardStart.transform.position.y, 0);
    }

    public void HiglightMoves(BoardCell cell) {
        for(int i = 0; i < cell.Piece.Moves.Length; i++) {
            for(int u = 0; u < cell.Piece.Moves[i].MovePositions.Length; u++) {
                Move move = cell.Piece.Moves[i];
                if(move.IsStartMove && cell.Piece.HasMoved) break;
                BoardCell cellToColor = GetCell(move.MovePositions[u] + cell.Position);
                if(cellToColor == null || cellToColor.Piece != null) break;
                cellToColor.SetHighlight(true);
                cellsHighlighted.Add(cellToColor);
            }
        }
    }

    public void ResetHiglights() {
        foreach(BoardCell cell in cellsHighlighted) {
            cell.SetHighlight(false);
        }
        cellsHighlighted.Clear();
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition) => GetWorldPosition(gridPosition.x, gridPosition.y);
    public void MovePiece(Vector2Int oldPosition, Vector2Int newPosition) {
        BoardCell oldCell = GetCell(oldPosition);
        BoardCell newCell = GetCell(newPosition);
        Piece pieceToMove = oldCell.Piece;
        pieceToMove.Move(GetWorldPosition(newPosition), newPosition);
        newCell.Piece = pieceToMove;
        oldCell.Piece = null;
    }
}
