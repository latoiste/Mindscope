using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueClick : MonoBehaviour, IPointerClickHandler
{
    public questionManager qManager;
    private bool canClick = false;

    private void OnEnable()
    {
        canClick = false;
        StartCoroutine(EnableClickWithDelay());
    }

    private IEnumerator EnableClickWithDelay()
    {
        yield return new WaitForSeconds(0.15f);
        canClick = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick && qManager != null)
        {
            canClick = false;

            // 1. Call FinishAnswer (either skips typing OR deactivates the panel)
            qManager.FinishAnswer();

            // 2. Only start cooldown if this GameObject is STILL active (i.e., we just skipped typing)
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(ClickCooldown());
            }
        }
    }

    private IEnumerator ClickCooldown()
    {
        yield return new WaitForSeconds(0.15f);
        canClick = true;
    }
}