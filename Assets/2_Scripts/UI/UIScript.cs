using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour       //인게임 UI관리
{
    public static UIScript instance;
    [SerializeField] Text killScore;
    int _killScore=0;

    [SerializeField] Text Second;
    [SerializeField] Text MSecond;

    [SerializeField] Text touchCount;
    int _touchCount = 0;


    public int maxScore = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        killScore.text = _killScore.ToString();
    }
    private void Update()
    {
        if(InGamManager.isPlaying)      //터치 카운트 증가
        {
            if(Input.GetMouseButtonDown(0))
            {
                _touchCount++;
                touchCount.text = _touchCount.ToString();
            }
        }
    }

   

    public void ResetScore()
    {
        _killScore = 0;
        killScore.text = _killScore.ToString();
        _touchCount = 0;
        touchCount.text = _touchCount.ToString();
    }

    public void BugKill(int _killscore)
    {
        _killScore+=_killscore;
        killScore.text = _killScore.ToString();
    }

    public int getScore()
    {
        return _killScore;
    }
    public int getTotal()
    {
        return _touchCount;
    }

    //결과창에 최고점수 참조용
    public int getMax()
    {       
        return maxScore;
    }

    public void setMaxScore(int data)
    {
        maxScore = data;
    }
    public void SetPlayTime(float _second,int _msecond)
    {
       
        Second.text = _second.ToString();
        if (_msecond < 10)
            MSecond.text = "0"+_msecond.ToString();
        else
            MSecond.text = _msecond.ToString();
    }

}
