using UnityEngine;
using System.Collections;
using TMPro;

public class questionManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string question;
        [TextArea(2, 5)] public string answer;
        [TextArea(2, 5)] public string clue; // Clue Opsional (kosongkan jika pertanyaan ini tidak memberikan clue)
    }

    [Header("Clue Board Reference")]
    public ClueBoard clueBoard; // Drag Object ClueBoard ke sini di Inspector

    [Header("Question GameObjects")]
    public GameObject[] questionObjects;

    [Header("Dialogue")]
    public GameObject dialoguePanel;
    public TMP_Text questionText;
    public TMP_Text answerText;

    [Header("Questions")]
    public Question[] questions;

    private int currentSet = 0;
    private int selectedCount = 0;
    private bool[] selectedQuestions;
    private bool canCloseDialogue = false;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        selectedQuestions = new bool[4];

        for (int i = 0; i < questionObjects.Length; i++)
        {
            if (questionObjects[i] == null)
                continue;

            int slot = i;

            Button btnScript = questionObjects[i].GetComponent<Button>();

            if (btnScript != null)
            {
                btnScript.onClick.RemoveAllListeners();
                btnScript.onClick.AddListener(() =>
                {
                    SelectQuestion(slot);
                });
            }
        }

        ShowQuestions();
    }

    void SelectQuestion(int slot)
    {
        if (selectedQuestions != null && slot < selectedQuestions.Length && selectedQuestions[slot])
        {
            return;
        }

        int questionIndex = currentSet * 4 + slot;

        if (questions == null || questionIndex >= questions.Length)
            return;

        selectedQuestions[slot] = true;
        selectedCount++;

        HideAllQuestions();

        if (questionText != null)
            questionText.text = questions[questionIndex].question;

        if (answerText != null)
            answerText.text = questions[questionIndex].answer;
            
        if (clueBoard != null && !string.IsNullOrEmpty(questions[questionIndex].clue))
        {
            clueBoard.ShowClue(questions[questionIndex].clue);
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        canCloseDialogue = false;
        StopAllCoroutines();
        StartCoroutine(EnableDialogueCloseDelay());
    }

    private IEnumerator EnableDialogueCloseDelay()
    {
        yield return new WaitForSeconds(0.3f);
        canCloseDialogue = true;
    }

    public void FinishAnswer()
    {
        if (!canCloseDialogue) return;

        canCloseDialogue = false;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (selectedCount >= 3)
        {
            ChangeQuestionSet();
        }
        else
        {
            ShowQuestions();
        }
    }

    void HideAllQuestions()
    {
        for (int i = 0; i < questionObjects.Length; i++)
        {
            if (questionObjects[i] != null)
                questionObjects[i].SetActive(false);
        }
    }

    void ShowQuestions()
    {
        if (questions == null || questions.Length == 0)
        {
            for (int i = 0; i < questionObjects.Length; i++)
            {
                if (questionObjects[i] != null)
                {
                    questionObjects[i].SetActive(true);

                    Button btnScript = questionObjects[i].GetComponent<Button>();
                    if (btnScript != null)
                        btnScript.Enable();
                }
            }
            return;
        }

        for (int i = 0; i < questionObjects.Length; i++)
        {
            if (questionObjects[i] == null)
                continue;

            int questionIndex = currentSet * 4 + i;

            if (questionIndex < questions.Length && !selectedQuestions[i])
            {
                questionObjects[i].SetActive(true);

                TMP_Text buttonText = questionObjects[i].GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.text = questions[questionIndex].question;
                }

                Button btnScript = questionObjects[i].GetComponent<Button>();
                if (btnScript != null)
                    btnScript.Enable();
            }
            else
            {
                questionObjects[i].SetActive(false);
            }
        }
    }

    void ChangeQuestionSet()
    {
        currentSet++;
        selectedCount = 0;
        selectedQuestions = new bool[4];

        ShowQuestions();
    }
}