using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PieceData 
{
    public enum Type
    {
        Pawn,
        King,
        Queen,
        Bishop,
        Rook,
        Knight
    }

    public enum Color
    {
        White,
        Black
    }

    public Type PieceType;
    public Color PieceColor;
    public Vector2Int Position;
}

[Serializable]
public class PiecePrefab
{
    public GameObject Prefab;
    public PieceData.Type Type;
    public PieceData.Color Color;

}
