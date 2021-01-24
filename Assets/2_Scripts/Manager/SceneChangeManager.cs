using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public enum eStateScene     //현재 씬의 상태를 저장하여 타이틍->인게임으로 넘어갈때 게임 준비동작 호출
    {
        StartScene=0,
        TitleScene,
        InGame,
    }
    eStateScene currentScene;

    //로딩화면 관련 프리팹 참조
    [SerializeField] GameObject goLoadingWindow;
    LoadingUI wndLoad;

    static SceneChangeManager _uniqeInstance;

    //씬이동시 진행 상황과 완료여부를 알기 위해 어싱크 사용
    AsyncOperation _asyncOpr;
    float timeCheck = 0;
    public static SceneChangeManager instance        //인스턴스화, 하이어래키 상에 유일하게 존재할때 사용 가능
    {
        get { return _uniqeInstance; }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartTitleScene("TitleScene");
        _uniqeInstance = this;
        DontDestroyOnLoad(gameObject);
        currentScene = eStateScene.StartScene;
    }

    private void LateUpdate()
    {
        if(_asyncOpr!=null) //씬 이동이 일어났을때
        {
            //로딩바 최산화
            wndLoad.SettingProgress(_asyncOpr.progress);    
           
            if (_asyncOpr.isDone)   //씬이동이 끝났으면
            {
                timeCheck += Time.deltaTime;
                if (timeCheck > 2)
                {
                    timeCheck = 0;
                    _asyncOpr = null;   //어싱크 변수 초기화
                    Destroy(wndLoad.gameObject);    //로딩창 제거
                    if(currentScene==eStateScene.InGame)    //인게임 상황이면
                        InGamManager._instance.WaitGame();  //게임 시작
                }
            }
        }
    }


    public void StartTitleScene(string SceneName)   //로딩창이 필요없느 스타트->타이틀
    {
        if (SceneName.CompareTo(eStateScene.TitleScene.ToString()) == 0)
            currentScene = eStateScene.TitleScene;
        else
            currentScene = eStateScene.InGame;

        if (currentScene == eStateScene.InGame)    //인게임 상황이면
            InGamManager._instance.WaitGame();  //게임 시작
        SceneManager.LoadSceneAsync(SceneName);
    }

    public void StartChangeScene(string SceneName)  //로딩창이 필요한 씬 이동들
    {
        if (SceneName.CompareTo(eStateScene.TitleScene.ToString()) == 0)
            currentScene = eStateScene.TitleScene;
        else
            currentScene = eStateScene.InGame;

        GameObject go = Instantiate(goLoadingWindow, transform);
        wndLoad = go.GetComponent<LoadingUI>();
        wndLoad.OpenWindow();
        _asyncOpr= SceneManager.LoadSceneAsync(SceneName);
    }


}
