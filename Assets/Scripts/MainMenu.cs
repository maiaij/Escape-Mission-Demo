using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool isLoad;
    public void PlayButton()
    {
        isLoad = false;
        SceneManager.LoadScene("DemoLevel");
    }

    public void LoadButton()
    {
        isLoad = true;
        SceneManager.LoadScene("DemoLevel");
    }

    public void RulesButton()
    {
        isLoad = false;
        SceneManager.LoadScene("Instructions");
    }

    public void CreditsButton()
    {
        isLoad = false;
        SceneManager.LoadScene("Credits");
    }

    public void BackButton()
    {
        isLoad = false;
        SceneManager.LoadScene("MainMenu");
    }

}