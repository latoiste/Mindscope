using UnityEngine;

public class InformationBoard : Board
{
    protected override async void OnBoardMoved(Board board, bool opening)
    {
        if (opening)
        {
            sprite.sortingOrder = 0;
        } else
        {
            await movingOp;
            sprite.sortingOrder = 5;
        }
    }
}