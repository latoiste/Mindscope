using UnityEngine;
using UnityEngine.SceneManagement;

public class gobackmainmenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void quit()
    {
        SceneManager.LoadScene(0);
    }
}
