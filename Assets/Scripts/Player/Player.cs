using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        Patient patient = GameManager.instance.GetSelectedPatient;
        Animator animator = GetComponentInChildren<Animator>();

        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(patient.controllerPath);
        if (controller == null)
        {
            Debug.LogError($"Controller file for {patient.id} not found");
            return;
        }
        
        animator.runtimeAnimatorController = controller;
    }
}