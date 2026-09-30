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

    private async void OnBoardMoved(Board board, bool opening)
    {
        if (board == activeBoard && !opening)
        {
            activeBoard = null;
            return;
        }

        if (!opening) return;

        DisableBoards();

        var oldActiveBoard = activeBoard;
        activeBoard = board;

        if (oldActiveBoard != null) await oldActiveBoard.CloseBoard();
        if (board.movingOp != null) await board.movingOp;
        
        EnableBoards();
    }

    private void DisableBoards()
    {
        foreach (var b in boards) b.Disable();
    }

    private void EnableBoards()
    {
        foreach (var b in boards) b.Enable();
    }

    void OnDestroy()
    {
        foreach (var b in boards)
        {
            b.onMoved.RemoveListener(OnBoardMoved);
        }
    }
}