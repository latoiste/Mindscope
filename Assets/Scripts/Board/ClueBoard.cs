using System;

public class ClueBoard : Board
{
    protected override void OnBoardMoved(Board _, bool opening)
    {
        sprite.sortingOrder = opening ? 0 : 100;
    }
}