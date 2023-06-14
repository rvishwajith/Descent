using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void SwitchToTutorialScene()
    {
        print("Switching to tutorial scene.");
        SceneManager.LoadScene(1);
    }
}
