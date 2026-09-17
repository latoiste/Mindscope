using UnityEngine;

public class checkedTracker : MonoBehaviour
{
    public int checkedBox;
    public int checkNeed;

    public GameObject diagnoseButton;
    public GameObject diagnoseBox;
    void Start()
    {
        checkedBox = 0;
        diagnoseButton.SetActive(false);
        diagnoseBox.SetActive(false);
    }

    public void checkStatus()
    {
        if (checkedBox == checkNeed)
        {
            diagnoseButton.SetActive(true);
            diagnoseBox.SetActive(true);
        }

        else
        {
            diagnoseButton.SetActive(false);
            diagnoseBox.SetActive(false);
        }
    }
    
}
