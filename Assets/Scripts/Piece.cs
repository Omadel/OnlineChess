using UnityEngine;

public class Piece : MonoBehaviour {
    public bool HasMoved => hasMoved;
    public Move[] Moves => moves;

    public PieceData Data;

    [SerializeField] private Move[] moves;
    private bool hasMoved = false;

    public void Move(Vector3 position, Vector2Int gridPosition) {
        hasMoved = true;
        transform.position = position;
        Data.Position = gridPosition;
    }
}

[System.Serializable]
public struct Move {
    public bool IsStartMove => isStartMove;
    public Vector2Int[] MovePositions => movePositions;

    [SerializeField] private bool isStartMove;
    [SerializeField] private Vector2Int[] movePositions;
}
