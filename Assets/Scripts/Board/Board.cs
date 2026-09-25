using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Board : MonoBehaviour
{
    
    [SerializeField] private Vector2 movedCanvasPos;
    [SerializeField] private float boardSpeed = 0.5f;
    
    protected Button toggleButton;
    protected SpriteRenderer sprite;
    protected bool moved;
    private bool isMoving;
    protected Vector2 originalCanvasPos { get; set; }
    public UnityEvent<Board, bool> onMoved; // opening: bool
    protected Task movingOp;

    protected abstract void OnBoardMoved(Board board, bool opening);

    protected virtual void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        toggleButton = GetComponentInChildren<Button>();

        toggleButton.onClick.AddListener(() => _ = ToggleBoard());
        onMoved.AddListener(OnBoardMoved);
        
        moved = false;
        originalCanvasPos = transform.position;
    }

    public async Task ToggleBoard()
    {
        if (isMoving) return;

        Vector2 pos = moved ? originalCanvasPos : movedCanvasPos;
        isMoving = true;

        movingOp = transform
            .DOMove(pos, boardSpeed)
            .SetEase(Ease.OutCubic)
            .AsyncWaitForCompletion();

        onMoved?.Invoke(this, !moved); // !moved == board opening
        await movingOp;

        movingOp = null;
    
        moved = !moved;
        isMoving = false;
    }

    public async Task OpenBoard()
    {
        if (!moved) await ToggleBoard();
    }

    public async Task CloseBoard()
    {
        if (moved) await ToggleBoard();
    }

    void OnDestroy()
    {
        onMoved.RemoveAllListeners();
    }
}