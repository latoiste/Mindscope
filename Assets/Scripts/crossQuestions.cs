using UnityEngine;
using TMPro;

public class crossQuestions : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject text;

    public void crossQuestion()
    {
        TextMeshProUGUI tmp = text.GetComponent<TextMeshProUGUI>();
        tmp.text = $"<s>{tmp.text}</s>";
    }
}
