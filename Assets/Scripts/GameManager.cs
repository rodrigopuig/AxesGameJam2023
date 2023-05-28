using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public bool deleteData;
    public GameObject mainMenu;
    public GameObject controlsUI;
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI scoreBoard;

    private string currentTeamName;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.000000001f;

        FinishLine.Finish += OnFinishLine;

        if(deleteData)
        {
            PlayerPrefs.DeleteAll();
        }

        mainMenu.SetActive(true);
        controlsUI.SetActive(true);
        countdown.gameObject.SetActive(false);

        string[] scores = PlayerPrefs.GetString("scores").Split("\n");
        List<Tuple<string, float>> scoreStruct = new List<Tuple<string, float>>();
        for(int i = 0; i < scores.Length; ++i)
        {
            if(!string.IsNullOrEmpty(scores[i]))
            {
                string name = scores[i].Substring(0, scores[i].IndexOf(":"));
                int i1 = scores[i].IndexOf(":");
                int i2 = scores[i].LastIndexOf("s");
                i2 -= i1;
                string score = scores[i].Substring(i1 + 1, i2 - 1);
                Tuple<string, float> s = new Tuple<string, float>(name, float.Parse(score));
                scoreStruct.Add(s);
            }
        }
        if (scoreStruct != null && scoreStruct.Count > 1)
        {
            scoreStruct = scoreStruct.OrderBy(i => i.Item2).ToList();
            // scoreStruct.Sort((x, y) =>
            // {
            //     return x.Item2 > y.Item2;
            // });
        }

        string finalScores = "";
        foreach(var ss in scoreStruct)
        {
            finalScores += "\n" + ss.Item1 + ": " + ss.Item2.ToString() + "s";
        }

        scoreBoard.text = finalScores;

        SoundController.instance.PlaySound(SFXid.meta);
    }

    public void SetTeamName(string name)
    {
        currentTeamName = name;
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);

        if(string.IsNullOrEmpty(currentTeamName))
        {
            currentTeamName = "(unnamed)";
        }

        SoundController.instance.PlaySound(SFXid.boton);

        StartCoroutine(StartRoutine());
    }

    private IEnumerator StartRoutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        countdown.gameObject.SetActive(true);
        controlsUI.SetActive(true);

        for(int i = 3; i > 0; --i)
        {
            Debug.Log(i.ToString());
            SoundController.instance.PlaySound(SFXid.countdown321);
            countdown.text = i.ToString();
            int sizeOffset = 0;
            do
            {
                countdown.fontSize++;
                sizeOffset++;
                yield return new WaitForSecondsRealtime(0.03f);
            }
            while(sizeOffset < 30);
            countdown.fontSize -= sizeOffset;
        }


        Time.timeScale = 1;
        startTime = Time.time;

        countdown.text = "0";

        {
            SoundController.instance.PlaySound(SFXid.countdown0);
            int sizeOffset = 0;
            do
            {
                countdown.fontSize += 50;
                sizeOffset += 50;
                yield return new WaitForSecondsRealtime(0.01f);
            }
            while(sizeOffset < 5000);
        }

        countdown.gameObject.SetActive(false);
    }

    private void OnFinishLine()
    {
        FinishLine.Finish -= OnFinishLine;

        EndGame();
    }

    private void EndGame()
    {
        // mainMenu.SetActive(true);

        float time = Time.time - startTime;

        string scores = PlayerPrefs.GetString("scores");
        scores += "\n" + currentTeamName + ": " + time + "s";

        PlayerPrefs.SetString("scores", scores);

        SceneManager.LoadScene(0);
    }
    
}
