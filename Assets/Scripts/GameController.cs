using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int FirstValue = 666;
    private GameObject FirstSign;
    private GameObject SecondSign;
    private int SecondValue = 666;
    public Vector3 StartPosition;
    public int Rows;
    public int Collumns;
    public float RowOffset;
    public float ColOffset;
    public bool CanFlip = true;
    public GameObject SignPrefab;
    List<int> NumbersToSet = new List<int>();
    List<int> DoubledNumbers = new List<int>();
    private AudioSource SoundService;
    public AudioClip[] BadSounds;
    public AudioClip[] GoodSounds;
    private int PreviousBadSound = -1;
    private int PreviousGoodSound = -1;

    private void Start()
    {
        SoundService = GameObject.FindGameObjectWithTag("SoundService").GetComponent<AudioSource>();
        int NumberOfSigns = Rows * Collumns;
        for (int i = 0; i < NumberOfSigns / 2; i++)
        {
            NumbersToSet.Add(i + 1);
        }
        DoubledNumbers = new List<int>(NumbersToSet);

        float CurrentOffsetZ = 0f;
        float CurrentOffsetX = 0f;
        int NumberOfAlreadyBuiltSigns = 0;
        for (int j = 0; j < NumberOfSigns; j++)
        {
            NumberOfAlreadyBuiltSigns++;
            GameObject NewSign = Instantiate(SignPrefab);
            NewSign.transform.position = StartPosition + new Vector3(CurrentOffsetX, 0, CurrentOffsetZ);
            int Decide = Random.Range(0, 2);
            if (Decide == 0)
            {
                TakeDouble(NewSign);
            } else
            {
                TakeOriginal(NewSign);
            }
            CurrentOffsetX += ColOffset;
            if (NumberOfAlreadyBuiltSigns % Collumns == 0)
            {
                CurrentOffsetX = 0f;
                CurrentOffsetZ -= RowOffset;
            }
        }
        NumbersToSet = null;
        DoubledNumbers = null;
    }

    private void TakeDouble(GameObject NewSign)
    {
        if (DoubledNumbers.Count > 0)
        {
            int RandomN = Random.Range(0, DoubledNumbers.Count);
            NewSign.transform.Find("Canvas").Find("Value").GetComponent<TMP_Text>().text = DoubledNumbers.ElementAt(RandomN).ToString();
            DoubledNumbers.RemoveAt(RandomN);
        } else
        {
            TakeOriginal(NewSign);
        }
    }
    private void TakeOriginal(GameObject NewSign)
    {
        if (NumbersToSet.Count > 0)
        {
            int RandomN = Random.Range(0, NumbersToSet.Count);
            NewSign.transform.Find("Canvas").Find("Value").GetComponent<TMP_Text>().text = NumbersToSet.ElementAt(RandomN).ToString();
            NumbersToSet.RemoveAt(RandomN);
        }
        else
        {
            TakeDouble(NewSign);
        }
    }

    IEnumerator CompareVariables()
    {
        if (FirstValue == SecondValue)
        {
            CanFlip = false;
            FirstSign.GetComponent<Renderer>().material.color = Color.yellow;
            FirstSign.transform.Find("Canvas").Find("Value").GetComponent<TMP_Text>().color = Color.green;
            SecondSign.GetComponent<Renderer>().material.color = Color.yellow;
            SecondSign.transform.Find("Canvas").Find("Value").GetComponent<TMP_Text>().color = Color.green;
            int RandSound = Random.Range(0, GoodSounds.Length);
            if (RandSound == PreviousGoodSound)
            {
                while (RandSound == PreviousGoodSound)
                {
                    RandSound = Random.Range(0, GoodSounds.Length);
                }
            }
            SoundService.PlayOneShot(GoodSounds[RandSound]);
            PreviousGoodSound = RandSound;
            yield return new WaitForSecondsRealtime(0.5f);
            SignRotate FirstSR = FirstSign.GetComponent<SignRotate>();
            SignRotate SecondSR = SecondSign.GetComponent<SignRotate>();
            FirstSR.Remove();
            SecondSR.Remove();
            FirstValue = 666;
            FirstSign = null;
            SecondValue = 666;
            SecondSign = null;
            Debug.Log(GameObject.FindGameObjectsWithTag("Sign").Length);
            if (GameObject.FindGameObjectsWithTag("Sign").Length > 2)
            {
                CanFlip = true;
            }
            else
            {
                yield return new WaitForSecondsRealtime(1);
                int Score = int.Parse(GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>().text);
                if (Score <= 100)
                {
                    Info.IsWin = true;
                }
                SceneManager.LoadScene("Win");
            }
        } else
        {
            SignRotate FirstSR = FirstSign.GetComponent<SignRotate>();
            SignRotate SecondSR = SecondSign.GetComponent<SignRotate>();
            CanFlip = false;
            int RandSound = Random.Range(0, BadSounds.Length);
            if (RandSound == PreviousBadSound)
            {
                while (RandSound == PreviousBadSound)
                {
                    RandSound = Random.Range(0, BadSounds.Length);
                }
            }
            SoundService.PlayOneShot(BadSounds[RandSound]);
            PreviousBadSound = RandSound;
            yield return new WaitForSecondsRealtime(0.5f);
            CanFlip = true;
            FirstSR.Unflip();
            SecondSR.Unflip();
        }
        FirstValue = 666;
    }

    public void AddValue(int Value, GameObject Sign)
    {
        if (FirstValue == 666)
        {
            FirstValue = Value;
            FirstSign = Sign;
        } else
        {
            SecondValue = Value;
            SecondSign = Sign;
            StartCoroutine(CompareVariables());
        }
    }

    public void RemoveValue(GameObject Sign)
    {
        if (SecondValue == 666)
        {
            FirstValue = 666;
            FirstSign = null;
        } else
        {
            if (Sign == FirstSign)
            {
                FirstSign = SecondSign;
                SecondSign = null;
                FirstValue = SecondValue;
                SecondValue = 666;
            } else
            {
                SecondValue = 666;
                SecondSign = null;
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Lobby");
        }
        if ((Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.F)))
        {
            {
                Debug.Log("Quitting!");
                Application.Quit();
            }
        }
    }
}
