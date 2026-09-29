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
        yield return new WaitForSeconds(0.2f);
        canClick = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (canClick && qManager != null)
        {
            canClick = false;
            qManager.FinishAnswer();
        }
    }
}