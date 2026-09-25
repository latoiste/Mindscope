using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string question;
        public string answer;
    }

    [Header("Question Buttons")]
    public UnityEngine.UI.Button[] questionButtons;

    [Header("Dialogue")]
    public GameObject dialoguePanel;
    public TMP_Text questionText;
    public TMP_Text answerText;

    [Header("Questions")]
    public Question[] questions;

    private int currentSet = 0;
    private int selectedCount = 0;
    private bool[] selectedQuestions;

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        selectedQuestions = new bool[4];

        for (int i = 0; i < questionButtons.Length; i++)
        {
            if (questionButtons[i] == null) continue;

            int slot = i;
            questionButtons[i].onClick.RemoveAllListeners();
            questionButtons[i].onClick.AddListener(() =>
            {
                SelectQuestion(slot);
            });
        }

        ShowQuestions();
    }

    void SelectQuestion(int slot)
    {
        if (selectedQuestions != null && slot < selectedQuestions.Length && selectedQuestions[slot])
            return;

        int questionIndex = currentSet * 4 + slot;

        if (questions == null || questionIndex >= questions.Length)
            return;

        selectedQuestions[slot] = true;
        selectedCount++;

        HideAllQuestions();

        if (questionText != null) questionText.text = questions[questionIndex].question;
        if (answerText != null) answerText.text = questions[questionIndex].answer;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
    }

    public void FinishAnswer()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

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
        for (int i = 0; i < questionButtons.Length; i++)
        {
            if (questionButtons[i] != null)
                questionButtons[i].gameObject.SetActive(false);
        }
    }

    void ShowQuestions()
    {
        if (questions == null || questions.Length == 0)
        {
            Debug.LogWarning("Array Questions masih kosong, tetapi tombol tetap dipaksa aktif.");
            for (int i = 0; i < questionButtons.Length; i++)
            {
                if (questionButtons[i] != null)
                {
                    questionButtons[i].gameObject.SetActive(true);
                    questionButtons[i].interactable = true;
                }
            }
            return;
        }

        for (int i = 0; i < questionButtons.Length; i++)
        {
            if (questionButtons[i] == null) continue;

            int questionIndex = currentSet * 4 + i;

            if (questionIndex < questions.Length && !selectedQuestions[i])
            {
                questionButtons[i].gameObject.SetActive(true);
                questionButtons[i].interactable = true;
            }
            else
            {
                questionButtons[i].gameObject.SetActive(false);
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