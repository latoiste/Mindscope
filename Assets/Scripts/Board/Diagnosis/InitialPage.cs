using UnityEngine;
using UnityEngine.Events;

public class InitialPage : MonoBehaviour
{
    [SerializeField] private Button diagnoseButton;
    [SerializeField] private Checkbox noMentalCondition;
    [SerializeField] private Checkbox insufficientEvidence;
    [SerializeField] private Checkbox hasMentalCondition;

    private Checkbox activeCheckbox;
    [SerializeField] private DiagnosisBoard diagnosisBoard;
    public UnityEvent<string> onDiagnosed = new();

    void Awake()
    {
        if (diagnosisBoard == null)
            diagnosisBoard = GetComponentInParent<DiagnosisBoard>();
        if (diagnosisBoard == null)
            diagnosisBoard = FindFirstObjectByType<DiagnosisBoard>();

        if (noMentalCondition != null)
            noMentalCondition.onClick.AddListener(() => OnCheckboxClicked(noMentalCondition, insufficientEvidence));

        if (insufficientEvidence != null)
            insufficientEvidence.onClick.AddListener(() => OnCheckboxClicked(insufficientEvidence, noMentalCondition));

        if (hasMentalCondition != null)
            hasMentalCondition.onClick.AddListener(OnHasConditionClicked);

        if (diagnoseButton != null)
            diagnoseButton.onClick.AddListener(Diagnose);
    }

    private void OnCheckboxClicked(Checkbox clicked, Checkbox other)
    {
        if (clicked.IsChecked)
        {
            if (other != null) other.SetChecked(false);
            activeCheckbox = clicked;
            SetDiagnoseButtonVisible(true);
        }
        else
        {
            activeCheckbox = null;
            SetDiagnoseButtonVisible(false);
        }
    }

    private void OnHasConditionClicked()
    {
        // Uncheck all boxes on this page
        if (hasMentalCondition != null) hasMentalCondition.SetChecked(false);
        if (noMentalCondition != null) noMentalCondition.SetChecked(false);
        if (insufficientEvidence != null) insufficientEvidence.SetChecked(false);
        activeCheckbox = null;
        SetDiagnoseButtonVisible(false);

        // Transition to disease page
        if (diagnosisBoard != null)
        {
            diagnosisBoard.SwitchPage("disease");
        }
        else
        {
            Debug.LogError("[InitialPage] DiagnosisBoard reference could not be found!");
        }
    }

    private void Diagnose()
    {
        if (activeCheckbox == null || !activeCheckbox.IsChecked || !gameObject.activeSelf) return;

        onDiagnosed?.Invoke(activeCheckbox.value);
    }

    void OnEnable()
    {
        activeCheckbox = null;

        // Ensure all colliders are active and clickable
        if (noMentalCondition != null)
        {
            noMentalCondition.Enable();
            noMentalCondition.SetChecked(false);
        }
        if (insufficientEvidence != null)
        {
            insufficientEvidence.Enable();
            insufficientEvidence.SetChecked(false);
        }
        if (hasMentalCondition != null)
        {
            hasMentalCondition.Enable();
            hasMentalCondition.SetChecked(false);
        }

        SetDiagnoseButtonVisible(false);
    }

    private void SetDiagnoseButtonVisible(bool visible)
    {
        if (diagnoseButton != null)
        {
            diagnoseButton.gameObject.SetActive(visible);
            if (visible) diagnoseButton.Enable();
        }
    }
}