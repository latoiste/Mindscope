using UnityEngine;

public class PauseBoard : Board
{
    protected override void OnBoardMoved(Board board, bool opening)
    {
        sprite.sortingOrder = opening ? 0 : 10;
    }
}