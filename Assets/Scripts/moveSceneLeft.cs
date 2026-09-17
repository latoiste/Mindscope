using UnityEngine;

public class moveSceneLeft : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject button, otherButton;
    public GameObject panel, otherPanel;
    public GameObject board;
    public openBoard openBoard;
    public Vector3 originalCanvasPos;
    public Vector3 moveAmount = new Vector3(-20, 0, 0);
    public Vector3 boardExtend = new Vector3(-2, 0, 0);
    public bool moved = false;
    public bool isFirstPanel;

    public moveSceneLeft theOtherPanel;

    void Start()
    {
        if (panel != null) originalCanvasPos = panel.transform.position;
    }

    public void moveObject()
    {
        Debug.Log($"fuck {this.name}");
        if (isFirstPanel)
        {
            if (Vector3.Distance(panel.transform.position, originalCanvasPos) < 0.01) moved = false;
            else moved = true;
        }
        else
        {
            if (Vector3.Distance(panel.transform.position, originalCanvasPos) < 0.01) moved = false;
            else if (theOtherPanel.moved) moved = false;
            else moved = true;
        }

        // if (openBoard.moved == true)
        // {
        //     moved = false;
        //     openBoard.moved = false;
        // }

        if (!moved)
        {
            board.transform.position = originalCanvasPos + moveAmount + boardExtend;
            panel.transform.position = originalCanvasPos + moveAmount;

            button.transform.position = originalCanvasPos + moveAmount;

            if (!isFirstPanel)
            {
                otherButton.transform.position = originalCanvasPos;
                otherPanel.transform.position = originalCanvasPos;
            }

            else
            {
                otherPanel.transform.position = originalCanvasPos + moveAmount;
                otherButton.transform.position = originalCanvasPos + moveAmount;
            }

            moved = true;
            theOtherPanel.moved = false;
        }

        else
        {
            board.transform.position = originalCanvasPos;
            panel.transform.position = originalCanvasPos;
            otherPanel.transform.position = originalCanvasPos;

            button.transform.position = originalCanvasPos;
            otherButton.transform.position = originalCanvasPos;

            moved = false;
        }

    }
}
