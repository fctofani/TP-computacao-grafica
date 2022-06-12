using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameController gc;

    public void ExitButton() {
        Application.Quit();
        Debug.Log("fechando jogo...");
    }

    public void StartGameNormal () {
        SceneManager.LoadScene("Game");
    }

    public void StartGameAdvanced () {
        SceneManager.LoadScene("Game2");
    }
}
