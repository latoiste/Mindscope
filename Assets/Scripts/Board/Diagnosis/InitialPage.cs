using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class InitialPage : MonoBehaviour
{
    [SerializeField] private Button diagnoseButton;
    [SerializeField] private Checkbox noMentalCondition;
    [SerializeField] private Checkbox insufficientEvidence;
    [SerializeField] private Checkbox hasMentalCondition;
    private Dictionary<string, Checkbox> checkboxMap;
    private Checkbox activeCheckbox;
    private DiagnosisBoard diagnosisBoard;
    private bool canCheck;
    public UnityEvent<string> onDiagnosed;

    void Awake()
    {
        canCheck = true;
        checkboxMap = new()
        {
            { "noCondition", noMentalCondition },
            { "insufficientEvidence", insufficientEvidence },
            { "hasCondition", hasMentalCondition },
        };
        diagnosisBoard = GetComponentInParent<DiagnosisBoard>();

        noMentalCondition.onClick.AddListener(() => OnChecked(noMentalCondition));
        insufficientEvidence.onClick.AddListener(() => OnChecked(insufficientEvidence));
        hasMentalCondition.onClick.AddListener(() => OnChecked(hasMentalCondition));

        hasMentalCondition.onClick.AddListener(() => diagnosisBoard.SwitchPage("disease"));

        diagnoseButton.onClick.AddListener(Diagnose);
    }

    private void OnChecked(Checkbox checkbox)
    {
        if (activeCheckbox != null)
        {
            activeCheckbox.SetChecked(false);
        }
        activeCheckbox = checkbox;
    } 

    private void Diagnose()
    {
        if (activeCheckbox == null || !gameObject.activeSelf) return;

        onDiagnosed.Invoke(activeCheckbox.value);
    }

    void OnEnable()
    {
        hasMentalCondition.SetChecked(false);
    }

    // private async Task OnChecked(Checkbox checkbox)
    // {
        // if (!canCheck) return;

        // canCheck = false;

        // var oldActiveCheckbox = activeCheckbox;
        // activeCheckbox = checkbox;

        // if (oldActiveCheckbox != null) {
        //     await oldActiveCheckbox.ToggleCheck();
        // }
        // // await checkbox.animatingOp;

        // canCheck = true;
    // }
}