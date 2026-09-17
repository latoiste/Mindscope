using UnityEngine;

public class InformationBoard : Board
{
    protected override void OnBoardMoved(Board board, bool opening)
    {
        sprite.sortingOrder = opening ? 0 : 100;
    }
}