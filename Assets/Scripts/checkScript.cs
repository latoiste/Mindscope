using UnityEngine;

public class checkScript : MonoBehaviour
{
    public Animator checkBoxAnim;
    private bool isClicked;
    

    public checkedTracker tracker;
    void Start()
    {
        isClicked = false;
    }

    // Update is called once per frame
    public void clickBox()
    {
        if (!isClicked)
        {
            checkBoxAnim.SetBool("isCheck", true);
            tracker.checkedBox++;
            isClicked = true;
        }

        else
        {
            checkBoxAnim.SetBool("isCheck", false);
            tracker.checkedBox--;
            isClicked = false;
        }

        tracker.checkStatus();
    }
}
