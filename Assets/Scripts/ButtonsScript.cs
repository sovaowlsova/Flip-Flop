using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsScript : MonoBehaviour
{
    public void Menu()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
