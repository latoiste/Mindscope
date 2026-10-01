using UnityEngine;
using UnityEngine.Events;

public class InitialPage : MonoBehaviour
{
    [SerializeField] private Button diagnoseButton;
    [SerializeField] private Checkbox noMentalCondition;
    [SerializeField] private Checkbox insufficientEvidence;
    [SerializeField] private Checkbox hasMentalCondition;

    private Checkbox activeCheckbox;
    private DiagnosisBoard diagnosisBoard;
    public UnityEvent<string> onDiagnosed;

    void Awake()
    {
        diagnosisBoard = GetComponentInParent<DiagnosisBoard>();

        noMentalCondition.onClick.AddListener(() => OnCheckboxClicked(noMentalCondition, insufficientEvidence));
        insufficientEvidence.onClick.AddListener(() => OnCheckboxClicked(insufficientEvidence, noMentalCondition));

        // Clicking "has mental condition" immediately sends the player to disease selection
        hasMentalCondition.onClick.AddListener(OnHasConditionClicked);

        diagnoseButton.onClick.AddListener(Diagnose);
    }

    private void OnCheckboxClicked(Checkbox clicked, Checkbox other)
    {
        // If the box was just checked
        if (clicked.IsChecked)
        {
            // Uncheck the other option so only 1 can be checked
            other.SetChecked(false);
            activeCheckbox = clicked;

            // Show diagnose button
            SetDiagnoseButtonVisible(true);
        }
        else // The box was unchecked
        {
            activeCheckbox = null;

            // Hide diagnose button
            SetDiagnoseButtonVisible(false);
        }
    }

    private void OnHasConditionClicked()
    {
        // Uncheck it so it does not stay checked if player returns later
        hasMentalCondition.SetChecked(false);

        // Force player to the disease diagnosis pages
        diagnosisBoard.SwitchPage("disease");
    }

    private void Diagnose()
    {
        if (activeCheckbox == null || !activeCheckbox.IsChecked || !gameObject.activeSelf) return;

        onDiagnosed?.Invoke(activeCheckbox.value);
    }

    void OnEnable()
    {
        // Reset everything whenever this page is shown
        activeCheckbox = null;

        if (noMentalCondition != null) noMentalCondition.SetChecked(false);
        if (insufficientEvidence != null) insufficientEvidence.SetChecked(false);
        if (hasMentalCondition != null) hasMentalCondition.SetChecked(false);

        // Hide diagnose button until player selects an option
        SetDiagnoseButtonVisible(false);
    }

    private void SetDiagnoseButtonVisible(bool visible)
    {
        if (diagnoseButton != null)
        {
            diagnoseButton.gameObject.SetActive(visible);
        }
    }
}