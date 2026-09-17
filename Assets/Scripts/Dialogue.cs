using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BehaviorDialogue
{
    public string behaviorName;
    [TextArea(2, 5)]
    public string[] lines;
}

public class Dialogue : MonoBehaviour
{
    private ButtonIndex clickedButton;

    public ButtonIndex Q1, Q2, Q3, Q4, Q5, Q6, Q7, Q8, Q9, Q10, Q11, Q12;
    public GameObject[] questionButtons;
    public GameObject[] scribbles;
    public GameObject[] questionsTexts;
    public int buttonCounter;
    public TextMeshProUGUI textComp;
    public float textSpeed = 0.05f;
    public List<BehaviorDialogue> behaviorDialogues;

    private string[] currentLines;
    private int index = 0;
    private BehaviorDialogue currentBehavior;
    private bool isTyping = false;
    public bool isStart;
    public bool isClicked = false;

    // other canvases
    public openBoard openBoard;
    public moveSceneLeft firstPanel;
    public GameObject firstButton;

    public moveSceneLeft secondPanel;
    public GameObject secondButton;

    // next dialogue button
    public GameObject dialogButton;
    public Button nextButton;
    private bool dialogClick = false;

    public Animator animation;

    public int behaviorInd = 0;

    private bool skipClickAbsorbed = false;

    private void Start()
    {
        buttonCounter = 0;
        questionButtons = new GameObject[12];
        questionsTexts = new GameObject[12];
        scribbles = new GameObject[12];
        for (int i = 1; i <= 12; i++)
        {
            questionButtons[i - 1] = GameObject.Find("thirdPanelCanvas/question" + i);
            if (i > 6) questionButtons[i - 1].SetActive(false);

            scribbles[i - 1] = GameObject.Find("thirdPanelCanvas/q" + i);
            scribbles[i - 1].SetActive(false);

            questionsTexts[i - 1] = GameObject.Find("thirdPanelCanvas/question" + i + "Text");
            if (i > 6) questionsTexts[i - 1].SetActive(false);
        }
        
        putToBehvr0();
    }

    public void assignBehvrDialogues()
    {
        switch (behaviorInd)
        {
            case 1:
                Q1.startInd = 0;
                Q1.endInd = 2;

                Q2.startInd = 3;
                Q2.endInd = 3;

                Q3.startInd = 4;
                Q3.endInd = 5;

                Q4.startInd = 6;
                Q4.endInd = 7;

                Q5.startInd = 8;
                Q5.endInd = 10;

                Q6.startInd = 11;
                Q6.endInd = 11;

                //SPECIFICS
                Q7.startInd = 12;
                Q7.endInd = 16;

                Q8.startInd = 17;
                Q8.endInd = 18;

                Q9.startInd = 19;
                Q9.endInd = 19;

                Q10.startInd = 20;
                Q10.endInd = 20;

                Q11.startInd = 21;
                Q11.endInd = 21;

                Q12.startInd = 22;
                Q12.endInd = 23;

                break;
        }
    }

    private void putToBehvr0()
    {
        isStart = false;
        behaviorInd = 0;
        currentBehavior = behaviorDialogues[behaviorInd];
        currentLines = currentBehavior.lines;

        if (currentLines == null) Debug.Log("hey");
        StartCoroutine(TypeLine());

        //random ind here
        behaviorInd = 1;
        assignBehvrDialogues();
    }

    public void closeAllPanels()
    {
        openBoard.moved = false;
        firstPanel.moved = false;
        secondPanel.moved = false;

        firstPanel.panel.transform.position = firstPanel.originalCanvasPos;
        firstPanel.otherPanel.transform.position = firstPanel.originalCanvasPos;
        firstPanel.board.transform.position = firstPanel.originalCanvasPos;

        firstButton.transform.position = firstPanel.originalCanvasPos;
        secondButton.transform.position = firstPanel.originalCanvasPos;
    }

    public void showHideDialogue()
    {
        if (buttonCounter > 1)
        {
            for (int i = 6; i < 12; i++)
            {
                questionButtons[i].SetActive(true);
                questionsTexts[i].SetActive(true);
            }

            for (int i = 0; i < 6; i++)
            {
                scribbles[i].SetActive(true);
                questionButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void OnButtonClicked(ButtonIndex clicked)
    {
        showHideDialogue();
        clickedButton = clicked;
        animation.SetInteger("isTalking", 1);
        buttonCounter++;
        StartDialogue();
    }


    public void StartDialogue()
    {


        if (!isClicked)
        {
            closeAllPanels();
            scribbles[clickedButton.buttonInd].SetActive(true);
            dialogButton.GetComponent<Button>().interactable = true;
            isClicked = true;
            isStart = true;
            textComp.text = string.Empty;
            index = clickedButton.startInd;

            // int randomIndex = Random.Range(1, behaviorDialogues.Count);
            currentBehavior = behaviorDialogues[behaviorInd];
            currentLines = currentBehavior.lines;

            Debug.Log("New behavior picked: " + currentBehavior.behaviorName);

            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine()
    {

        animation.SetInteger("isTalking", 1);
        isTyping = true;
        textComp.text = "";

        foreach (char c in currentLines[index].ToCharArray())
        {


            textComp.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        animation.SetInteger("isTalking", 0);
    }

    public void onClick()
    {
        // CASE 1: Still typing — user clicked to finish line early
  
  
        if (isTyping)
        {
            Debug.Log("hi");
            StopAllCoroutines();
            textComp.text = currentLines[index];
            isTyping = false;

            if(clickedButton.buttonInd < 6) animation.SetInteger("isTalking", 0);
            skipClickAbsorbed = true; // absorb next click because user skipped
            return;
        }

        /*else if (skipClickAbsorbed)
        {
            skipClickAbsorbed = false;
            dialogButton.GetComponent<Button>().interactable = true;
            return;
        }*/

        else
        {
            NextLine();
        }

        // CASE 2: Absorb the click that immediately follows a skip

        // CASE 3: Move to next line
    
    }

    public void NextLine()
    {
        if (isStart && dialogButton.GetComponent<Button>().interactable)
        {
            if (index < clickedButton.endInd)
            {
                index++;
                StartCoroutine(TypeLine());
            }
            else
            {
                Debug.Log("koc");
                index = 0;
                dialogButton.GetComponent<Button>().interactable = false;
                clickedButton.GetComponent<Button>().interactable = false;
                isClicked = false;
                animation.SetInteger("isTalking", 0);
            }
        }
    }
}
