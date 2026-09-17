using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour
{
    private Board activeBoard;
    private List<Board> boards;
    void Start()
    {
        boards = new();
        Scene scene = SceneManager.GetActiveScene();
        
        GameObject[] rootGo = scene.GetRootGameObjects();
        
        foreach (var go in rootGo)
        {
            Board[] boardList = go.GetComponentsInChildren<Board>();
            boards.AddRange(boardList);
        }

        foreach (var b in boards)
        {
            b.onMoved.AddListener(OnBoardMoved);
        }
    }

    private void OnBoardMoved(Board board, bool opening)
    {
        if (board == activeBoard && !opening)
        {
            activeBoard = null;
            return;
        }

        if (activeBoard != null) _ = activeBoard.CloseBoard();
        activeBoard = board;
    }

    void OnDestroy()
    {
        foreach (var b in boards)
        {
            b.onMoved.RemoveListener(OnBoardMoved);
        }
    }
}