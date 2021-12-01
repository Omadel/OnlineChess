using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardCell : MonoBehaviour, IPointerClickHandler {
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public bool IsHighlighted => isHighlighted;

    public Vector2Int Position;
    public Piece Piece;
    public Action<BoardCell> OnClick;


    private bool isHighlighted;
    private SpriteRenderer spriteRenderer;

    private void Awake() => spriteRenderer = GetComponent<SpriteRenderer>();

    public void SetHighlight(bool highlighted) {
        isHighlighted = highlighted;
        spriteRenderer.color = highlighted ? Color.red : Color.white;
    }

    public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(this);
}
