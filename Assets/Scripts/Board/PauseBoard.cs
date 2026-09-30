using UnityEngine;

public class PauseBoard : Board
{
    protected override async void OnBoardMoved(Board board, bool opening)
    {
        if (opening)
        {
            SetSortingLayerName("openingBoard");
            await movingOp;
            SetSortingLayerName("board");
            sprite.sortingOrder = 0;
        } else
        {
            SetSortingLayerName("closingBoard");
            await movingOp;
            SetSortingLayerName("board");
            sprite.sortingOrder = 5;
        }
    }

    private void SetSortingLayerName(string layerName)
    {
        int layerId = SortingLayer.NameToID(layerName);

        sprite.sortingLayerID = layerId;
    }
}