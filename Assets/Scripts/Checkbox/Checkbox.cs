using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Checkbox : Button
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Sprite checkedSprite;
    [SerializeField] private Sprite uncheckedSprite;
    [SerializeField] public string value;
    private bool isChecked;
    private bool canCheck;
    public UnityEvent onChecked;

    protected override void Awake()
    {
        base.Awake();

        isChecked = false;
        canCheck = true;

        onClick.AddListener(ToggleCheck);
    }

    private void ToggleCheck()
    {
        sprite.sprite = isChecked ? uncheckedSprite : checkedSprite;
        isChecked = !isChecked;
    }

    public void SetChecked(bool checkedValue)
    {
        sprite.sprite = checkedValue ? checkedSprite : uncheckedSprite;
        isChecked = checkedValue;
    }

    // private Task ToggleCheck()
    // {
    //     if (!canCheck) return Task.CompletedTask;
        
    //     onChecked?.Invoke();

    //     var tcs = new TaskCompletionSource<bool>();
        
    //     StartCoroutine(ToggleAnimation(tcs));

    //     return tcs.Task;
    // }

    // private IEnumerator ToggleAnimation(TaskCompletionSource<bool> tcs)
    // {
    //     canCheck = false;
    //     animator.Play(isChecked ? "onUncheck" : "onCheck");

    //     isChecked = !isChecked;

    //     yield return null;

    //     AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

    //     yield return new WaitForSeconds(state.length);

    //     canCheck = true;
    //     tcs.SetResult(true);
    // }

    void OnDestroy()
    {
        onClick.RemoveAllListeners();
        onChecked.RemoveAllListeners();
    }
}