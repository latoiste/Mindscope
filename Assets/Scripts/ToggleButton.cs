using System;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    public UnityEvent onClick;

    void OnMouseDown()
    {
        onClick?.Invoke();
    }
}