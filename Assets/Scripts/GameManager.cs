using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string correctOutcome;

    void Awake()
    {
        correctOutcome = "mdd"; // ni hardcode dulu, storage buat store info gini2an nanti aja la
    }

    public void Diagnose(string outcome)
    {
        if (outcome == correctOutcome)
        {
            SceneManager.LoadScene("WinScreen");
        } else
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }
}