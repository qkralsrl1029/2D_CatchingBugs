using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class InGamManager : MonoBehaviour
{
    [SerializeField] Image _msgTitle;
    float checkTime = 0;
    generatorController _wormGC;
    [SerializeField] ResultWindow _result;
    [SerializeField] Database theData;
    public static int stageLevel = 0;

    [SerializeField] GameObject Stages;
    [SerializeField] GameObject mapObj;
    [SerializeField] Sprite[] tileMap;
    public static bool isStageSelected = false;

    public enum eFlowState      //게임진행 상황 
    {
        none = 0,
        WAIT = 1,
        GO,
        PLAY,
        TIMEOVER,
        RESULT
    }

    float _PlayTime = 0;    
    public bool isEnd = false;
    public static bool isPlaying = false;

    eFlowState curentState;
    static InGamManager _uniqeInstan;

    public static InGamManager _instance        //인스턴스화, 하이어래키 상에 유일하게 존재할때 사용 가능
    {
        get { return _uniqeInstan; }
    }
    // Start is called before the first frame update
    void Start()
    {
        _uniqeInstan = this;
        //WaitGame(); 씬 로딩이 끝난 후 실행, sceneChangeManager
        GameObject go = GameObject.FindGameObjectWithTag("WormGenerator");
        _wormGC = go.GetComponent<generatorController>();
    }


    private void LateUpdate()
    {
        switch(curentState)     //현재 state에 맞춰 동작 실행
        {
            case eFlowState.WAIT:
                
                if (isStageSelected)    //스테이지가 선택됐을경우
                {
                    ActivateTitle(true, "Wait...");
                    checkTime += Time.deltaTime;
                    if (checkTime >= 3)     //3초뒤에 게임 시작
                        GoGame();
                }
                break;
            case eFlowState.GO:
                checkTime += Time.deltaTime;
                if (checkTime >= 1)
                    PlayGame();
                break;
            case eFlowState.PLAY:
                _PlayTime -= Time.deltaTime;    //시간 감소
                CalcPlayTime();                 //남은시간 표시
                if (_PlayTime < 0)              //시간끝나면 타임오버
                    TimeoverGame();
                break;
            case eFlowState.TIMEOVER:
                checkTime += Time.deltaTime;
                isStageSelected = false;
                if (checkTime >= 1.5f)
                    ResultGame();
                break;

        }
    }

    public void ActivateTitle(bool isEnable=false,string msg="")    //타이틀 메시지 출력
    {
        _msgTitle.gameObject.SetActive(isEnable);
        Text txt = _msgTitle.transform.GetChild(0).GetComponent<Text>();
        txt.text = msg;
    }

    public void CalcPlayTime()      //소수점 두째자리까지 남은 시간 표시
    {
        float _msecond = (_PlayTime % 1);
        float _second = _PlayTime - _msecond;

        int temp = (int)(_msecond * 100);
        UIScript.instance.SetPlayTime(_second, temp);
    }


    //스테이트별 함수
    public void WaitGame()
    {
        Stages.SetActive(true);         //스테이지 선택창 표시
        SoundManager.instance.StopBGM();
        curentState = eFlowState.WAIT;
        _PlayTime = 60f;                //플레이타임 설정
        ActivateTitle();
    }
    public void GoGame()
    {
        curentState = eFlowState.GO;
        ActivateTitle(true, "GO!");
        checkTime = 0;
    }
    public void PlayGame()
    {
        SoundManager.instance.PlayBGM(1);   
        isPlaying = true;
        curentState = eFlowState.PLAY;
        ActivateTitle();            //상태바 제거
        checkTime = 0;
        _wormGC.startSpawn(1,4);    //벌레 생성 시작
    }
    public void TimeoverGame()
    {
        isPlaying = false;
        curentState = eFlowState.TIMEOVER;
        SoundManager.instance.StopBGM();
        ActivateTitle(true, "Time OVER!");  //문구 교체
        isEnd = true;                       //스태틱형 변수로 게임이 종료되었음을 각 객체에 알림
        _wormGC.StopSpawn();                //벌레 생성 중지
        _PlayTime = 0f;                     //플레이타임 초기화
        checkTime = 0;
        //생성된 벌레들 제거
        BugControl[] bugs = GameObject.FindObjectsOfType<BugControl>();
        for (int i = 0; i < bugs.Length; i++)
            Destroy(bugs[i].gameObject);
        

        //현재점수가 최고점수보다 높을경우 DB에 최신화
        if (UIScript.instance.getScore() > theData.getScore(stageLevel))
        {
            theData.SaveScore(stageLevel, UIScript.instance.getScore());
            ResultWindow.isHigherThanTop = true;
        }
    }

    //스테이지 선택
    public void stageSelect(int level)
    {
        SoundManager.instance.PlaySFX(SoundManager.eEffectType.BUTTON);
        isStageSelected = true;
        mapObj.GetComponent<SpriteRenderer>().sprite = tileMap[level];  //맵 배경 교체
        generatorController.uniqueBugCount = (level+1)*2;                     //벌레 생성패턴 교체
        Stages.SetActive(false);
    }


    public void ResultGame()
    {
        checkTime = 0;
        curentState = eFlowState.RESULT;

        UIScript.instance.setMaxScore(theData.getScore(stageLevel));    //해당 스테이지의 최고점수 참조
        ActivateTitle();
        _result.showResult();
    }
}
