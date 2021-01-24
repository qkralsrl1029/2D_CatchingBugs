using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Database : MonoBehaviour       //최고점수 저장, playerprefs사용
{
    public int[] score;
    [SerializeField] Text[] scores;         //스테이지 선택창에서 보여주는 각 스테이지별 최고점수들

    private void Awake()
    {
        LoadScore();
    }

    private void Start()
    {
        setScore();
    }
    public void SaveScore(int _index,int _score)
    {
        score[_index] = _score;

        PlayerPrefs.SetInt("ScoreEasy", score[0]);
        PlayerPrefs.SetInt("ScoreNormal", score[1]);
        PlayerPrefs.SetInt("ScoreHard", score[2]);
    }

    public void LoadScore()
    {
        if(PlayerPrefs.HasKey("ScoreEasy"))
        {
            score[0] = PlayerPrefs.GetInt("ScoreEasy");
            score[1] = PlayerPrefs.GetInt("ScoreNormal");
            score[2] = PlayerPrefs.GetInt("ScoreHard");
        }
    }

    public void setScore()
    {
        if (PlayerPrefs.HasKey("ScoreEasy"))
        {
            scores[0].text = score[0].ToString();
            scores[1].text = score[1].ToString();
            scores[2].text = score[2].ToString();
        }
    }

    public int getScore(int index)
    {
        if (PlayerPrefs.HasKey("ScoreEasy"))
        {
            return score[index];
        }
        else
            return 0;
    }
}
