using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public float BlackoutTime;
    public GameObject Background;

    public void StartGame()
    {
        StartCoroutine(Black());
    }

    private IEnumerator Black()
    {
        Material BackgroundMaterial = Background.GetComponent<Renderer>().material;
        Color32 StartColor = BackgroundMaterial.color;
        Color32 EndColor = new Color32(0, 0, 0, 1);
        float t = 0.0f;
        while (t < BlackoutTime)
        {
            t += Time.deltaTime;
            BackgroundMaterial.color = Color32.Lerp(StartColor, EndColor, t/BlackoutTime);
            yield return null;
        }
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
