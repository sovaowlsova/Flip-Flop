using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignRotate : MonoBehaviour
{
    public float FlipTime;
    public float Height;
    public bool IsFlipping;
    private float StartPosition;
    private float FinalPosition;
    public bool IsLocked = false;
    private GameController Controller;
    private TMP_Text Score;
    public AudioClip SignSound;
    private AudioSource SoundService;
    public AudioClip SignBackSound;
    private bool IsAvailable = true;
    public float FlyAwayTime;

    private void Start()
    {
        FinalPosition = transform.position.y + Height;
        StartPosition = transform.position.y;
        Controller = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>();
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
        SoundService = GameObject.FindGameObjectWithTag("SoundService").GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (!IsLocked && (Controller.CanFlip == true) && IsAvailable)
        {
            IsFlipping = !IsFlipping;
            StartCoroutine(Meow());
        }
    }

    public IEnumerator Meow()
    {
        if (IsFlipping)
        {
            IsLocked = true;
            SoundService.PlayOneShot(SignSound);
            int CurrentScore = int.Parse(Score.text);
            int NewScore = CurrentScore + 1;
            Score.text = NewScore.ToString();
            float t = 0.0f;
            while (t <= FlipTime)
            {
                t += Time.deltaTime;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(StartPosition, FinalPosition, t / FlipTime), transform.position.z);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(0, 180, t / FlipTime));
                yield return null;
            }
            IsLocked = false;
            Controller.AddValue(int.Parse(transform.Find("Canvas").Find("Value").GetComponent<TMP_Text>().text), gameObject);
        } else
        {
            SoundService.PlayOneShot(SignBackSound);
            IsLocked = true;
            float t = 0.0f;
            float startposition = transform.position.y;
            while (t <= FlipTime)
            {
                t += Time.deltaTime;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(startposition, StartPosition, t / FlipTime), transform.position.z);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Lerp(180, 0, t / FlipTime));
                yield return null;
            }
            Controller.RemoveValue(gameObject);
            IsLocked = false;
        }
    }

    public void Unflip()
    {
        IsFlipping = false;
        StartCoroutine(Meow());
    }

    public void Remove()
    {
        IsAvailable = false;
        StartCoroutine(FlyAway());
    }

    private IEnumerator FlyAway()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Vector3 EndPos = transform.position + new Vector3(10, 0, 0);
        Vector3 StartPos = transform.position;
        float t = 0.0f;
        while (t < FlyAwayTime)
        {
            t += Time.deltaTime;
            transform.position = new Vector3(Mathf.Lerp(StartPos.x, EndPos.x, t / FlyAwayTime), transform.position.y, transform.position.z);
            yield return null;
        }
        GameObject.Destroy(gameObject);
    }

}
