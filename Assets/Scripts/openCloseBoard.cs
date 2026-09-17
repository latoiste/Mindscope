using UnityEngine;

public class openBoard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public moveSceneLeft moveSceneLeft, moveSceneLeft2;

    public GameObject board;
    private Vector3 originalBoardPos;
    public bool moved = false;
    public Vector3 moveAmount = new Vector3(0, 18, 0);
    public Vector3 openedPanelPos = new Vector3(-22, 0, 0);

    void Start()
    {
        if (board != null) originalBoardPos = board.transform.position + openedPanelPos;
    }

    public void MoveBoard()
    {
        if (!moved)
        {
            board.transform.position = originalBoardPos + moveAmount;   
        } else
        {
            board.transform.position = originalBoardPos;
        }
        moved = !moved;
        // Debug.Log("AAAAAAAAAA");
        // if (Vector3.Distance(board.transform.position, originalBoardPos) < 0.01) moved = false;
        // else moved = true;

        // if (!moved)
        // {
        //     Debug.Log("kebuak");
        //     moveSceneLeft.moved = false;
        //     moveSceneLeft2.moved = false;

        //     board.transform.position = originalBoardPos + moveAmount;
        //     moved = true;
        // }

        // else
        // {
        //     Debug.Log("kwtutup");
        //     moveSceneLeft.moved = true;
        //     moveSceneLeft2.moved = true;
        //     board.transform.position = originalBoardPos;
        //     moved = false;
        // }
    }
}
