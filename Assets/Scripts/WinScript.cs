using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    public TMP_Text CodeWord;
    public TMP_Text Surrender;
    public GameObject SurrenderButtonImage;
    public AudioSource SoundService;
    public AudioClip WinSound;
    public AudioClip LoseSound;
    public AudioClip Hymn;

    private void Start()
    {
        if (Info.IsWin)
        {
            SurrenderButtonImage.GetComponent<Image>().color = Color.green;
            Surrender.text = "Закончить";
            CodeWord.text = "\r\n<i>Кодовое слово:<color=#04ff00> Серобуромалиновый </i></color>";
            StartCoroutine(PlayHymn());
        } else
        {
            SoundService.PlayOneShot(LoseSound);
        }
    }

    private IEnumerator PlayHymn()
    {
        SoundService.PlayOneShot(WinSound);
        yield return new WaitForSecondsRealtime(3);
        SoundService.PlayOneShot(Hymn);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Lobby");
    }
    public void Again()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
