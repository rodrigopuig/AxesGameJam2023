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
    public TextMeshProUGUI scoreBoard;

    private string currentTeamName;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;

        FinishLine.Finish += OnFinishLine;

        if(deleteData)
        {
            PlayerPrefs.DeleteAll();
        }

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

        string finalScores = "Best times:";
        foreach(var ss in scoreStruct)
        {
            finalScores += "\n" + ss.Item1 + ": " + ss.Item2.ToString() + "s";
        }

        scoreBoard.text = finalScores;
    }

    public void SetTeamName(string name)
    {
        currentTeamName = name;
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1;
        startTime = Time.time;

        if(string.IsNullOrEmpty(currentTeamName))
        {
            currentTeamName = "(unnamed)";
        }
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
