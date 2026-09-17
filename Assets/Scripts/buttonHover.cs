using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;

    public void OnPointerEnter(PointerEventData eventData)//checks for hover
    {
        animator.SetBool("isHover", true);//change animator parameter to run hover anim
            
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("isHover", false);
    }
}
