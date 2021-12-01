using System.Collections.Generic;
using UnityEngine;

public enum MessageType { Move }

[RequireComponent(typeof(Connect))]
public class MainGame : MonoBehaviour {
    public static MainGame Instance;

    public List<PieceData> StartPieces;
    public PiecePrefab[] PrefabPieces;
    public ChessBoard ChessBoard;

    private enum GameStep { None, PieceSelected }

    private GameStep _step;
    private Piece _currentPiece;
    private BoardCell _currentCell;


    private void Awake() => Instance = this;

    private void Start() {
        ChessBoard.Initialize();
        ChessBoard.OnBoardCellClicked += OnCellClicked;

        foreach(PieceData pieceData in StartPieces) {
            GameObject pieceGo = GameObject.Instantiate(GetPrefab(pieceData),
                ChessBoard.GetWorldPosition(pieceData.Position), Quaternion.identity);
            Piece piece = pieceGo.GetComponent<Piece>();
            piece.Data = pieceData;
            BoardCell cell = ChessBoard.GetCell(piece.Data.Position);
            cell.Piece = piece;
        }
    }

    private void OnCellClicked(BoardCell cell) {
        if(_step == GameStep.None) {
            SelectPiece(cell);
        } else if(_step == GameStep.PieceSelected) {
            MovePiece(cell);
        }
    }

    private void SelectPiece(BoardCell cell) {
        if(cell.Piece != null) {
            _currentPiece = cell.Piece;
            _currentCell = cell;
            _step = GameStep.PieceSelected;
            ChessBoard.HiglightMoves(cell);
        } else {
            ChessBoard.ResetHiglights();
        }
    }

    private void MovePiece(BoardCell cell) {
        if(!cell.IsHighlighted) {
            _step = GameStep.None;
        } else if(_currentPiece != null) {
            Connect.Send(nameof(MessageType.Move), _currentPiece.Data.Position.x, _currentPiece.Data.Position.y, cell.Position.x, cell.Position.y);
            _step = GameStep.None;
        }
        ChessBoard.ResetHiglights();
    }

    private GameObject GetPrefab(PieceData data) {
        foreach(PiecePrefab prefabPiece in PrefabPieces) {
            if(data.PieceType == prefabPiece.Type && data.PieceColor == prefabPiece.Color)
                return prefabPiece.Prefab;
        }

        throw new System.Exception($"Can't find type {data.PieceType} and {data.PieceColor}");
    }




}
