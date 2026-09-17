using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuselec : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
}
