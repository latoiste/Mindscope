using UnityEngine;
using System.Collections;
using TMPro;

public class questionManager : MonoBehaviour
{
    [Header("Clue Board Reference")]
    public ClueBoard clueBoard;

    [Header("Diagnosis Board")]
    public DiagnosisBoard diagnosisBoard;

    [Header("Converse / Start Interview")]
    [SerializeField] private Button converseButton;

    [Header("Question GameObjects")]
    public GameObject[] questionObjects;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TMP_Text questionText;
    public TMP_Text answerText;

    [Header("Character Animation")]
    [SerializeField] private Animator characterAnimator; // Drag Hal here
    [SerializeField] private string talkingBoolName = "istalking"; // Matches your animator parameter

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.035f;

    [Header("Dialogue Segments")]
    [SerializeField] private string segmentSeparator = "|";

    private string[] answerSegments;
    private int currentSegment = 0;
    private Question[] questions;
    private int currentSet = 0;
    private int selectedCount = 0;
    private int totalSelectedCount = 0;
    private bool[] selectedQuestions;
    private bool canCloseDialogue = false;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string activeParamName = null;

    private void Awake()
    {
        // Prevent Unity Inspector from keeping 0 if newly added
        if (typingSpeed <= 0f)
        {
            typingSpeed = 0.035f;
        }
    }

    private void Start()
    {
        questions = GameManager.instance.GetQuestions;

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

        if (converseButton != null)
        {
            converseButton.gameObject.SetActive(true);
            converseButton.onClick.RemoveAllListeners();
            converseButton.onClick.AddListener(StartInterview);

            HideAllQuestions();
        }
        else
        {
            ShowQuestions();
        }
    }

    public void StartInterview()
    {
        if (converseButton != null)
            converseButton.gameObject.SetActive(false);

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
        totalSelectedCount++;

        HideAllQuestions();

        if (questionText != null)
            questionText.text = questions[questionIndex].question;

        if (clueBoard != null && !string.IsNullOrEmpty(questions[questionIndex].clue))
        {
            clueBoard.ShowClue(questions[questionIndex].clue);
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        canCloseDialogue = false;

        // Start typing routine
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        string fullAnswer = questions[questionIndex].answer;

        if (string.IsNullOrEmpty(fullAnswer))
            fullAnswer = "...";

        answerSegments = fullAnswer.Split(
            new string[] { segmentSeparator },
            System.StringSplitOptions.None
        );

        currentSegment = 0;

        StartCurrentSegment();
            }
            private void StartCurrentSegment()
        {
            if (answerSegments == null ||
                currentSegment >= answerSegments.Length)
                return;

            canCloseDialogue = false;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            typingCoroutine = StartCoroutine(
                TypeAnswerRoutine(answerSegments[currentSegment].Trim())
            );
        }
            private IEnumerator TypeAnswerRoutine(string fullText)
            {
                isTyping = true;
                SetTalkingState(true);

                if (answerText != null)
                {
                    if (string.IsNullOrEmpty(fullText))
                    {
                        fullText = "...";
                    }

                    answerText.text = fullText;
                    answerText.ForceMeshUpdate(); // Forces TMP to calculate character count immediately

                    int totalCharacters = answerText.textInfo.characterCount;
                    if (totalCharacters <= 0)
                    {
                        totalCharacters = fullText.Length;
                    }

                    answerText.maxVisibleCharacters = 0;

                    float speed = typingSpeed <= 0f ? 0.035f : typingSpeed;

                    for (int i = 1; i <= totalCharacters; i++)
                    {
                        answerText.maxVisibleCharacters = i;
                        yield return new WaitForSeconds(speed);
                    }

                    answerText.maxVisibleCharacters = int.MaxValue;
                }

                isTyping = false;
                SetTalkingState(false);
                typingCoroutine = null;

                StartCoroutine(EnableDialogueCloseDelay());
    }

    private void SetTalkingState(bool isTalking)
    {
        if (characterAnimator == null)
        {
            Debug.LogWarning("[questionManager] Character Animator slot is EMPTY in the Inspector! Please assign Hal's Animator.");
            return;
        }

        // Auto-detect the parameter casing if not already found
        if (activeParamName == null)
        {
            if (HasParam(characterAnimator, talkingBoolName)) activeParamName = talkingBoolName;
            else if (HasParam(characterAnimator, "istalking")) activeParamName = "istalking";
            else if (HasParam(characterAnimator, "isTalking")) activeParamName = "isTalking";
            else if (HasParam(characterAnimator, "IsTalking")) activeParamName = "IsTalking";
            else
            {
                Debug.LogWarning($"[questionManager] Could not find '{talkingBoolName}' on {characterAnimator.name}'s Animator Controller!");
                return;
            }
        }

        characterAnimator.SetBool(activeParamName, isTalking);
    }

    private bool HasParam(Animator anim, string paramName)
    {
        foreach (AnimatorControllerParameter p in anim.parameters)
        {
            if (p.name == paramName) return true;
        }
        return false;
    }

    private IEnumerator EnableDialogueCloseDelay()
    {
        yield return new WaitForSeconds(0.2f);
        canCloseDialogue = true;
    }

    public void FinishAnswer()
{
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            if (answerText != null)
                answerText.maxVisibleCharacters = int.MaxValue;

            isTyping = false;
            SetTalkingState(false);

            canCloseDialogue = false;
            StartCoroutine(EnableDialogueCloseDelay());
            return;
        }

        if (!canCloseDialogue)
            return;

        canCloseDialogue = false;

        if (answerSegments != null &&
            currentSegment < answerSegments.Length - 1)
        {
            currentSegment++;
            StartCurrentSegment();
            return;
        }

        SetTalkingState(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (totalSelectedCount >= 6)
        {
            if (diagnosisBoard != null)
                diagnosisBoard.EnableBoard();

            return;
        }

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