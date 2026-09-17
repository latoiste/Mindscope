using UnityEngine;
using UnityEngine.SceneManagement;
public class clickDiagnose : MonoBehaviour
{
    //canvasdialogue behavior here

    public Animator checkBox;

    public Dialogue dialogue;
    public buttonBehavior buttonInd;

    public void onClick()
    {
        checkBox.SetBool("isClicked", true);
        
        Debug.Log(buttonInd.behaviorInd);
        Debug.Log(dialogue.behaviorInd);

        if (buttonInd.behaviorInd == dialogue.behaviorInd)
        {

            SceneManager.LoadScene(2);
        }

        else
        {
            SceneManager.LoadScene(3);
        }
    }
    
}
