using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour      //타이틀 화면 관리 매니저
{
    public static TitleSceneManager instance;       //인스턴스화

    [SerializeField] Text txtTitle;
    [SerializeField] Color[] colors;                //실시간별로 타이틀 글자 변화
    int colorIndex = 0;
    float timeCheck = 0;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        SoundManager.instance.PlayBGM(0);
        txtTitle.CrossFadeColor(colors[(colorIndex++) % colors.Length], 2.5f, true, false);
    }

    // Update is called once per frame
    void Update()
    {
        timeCheck += Time.deltaTime;
        if (timeCheck > 4)  //4초마다 색 변화
        {
            timeCheck = 0;
            //2.5초동안 변화 1.5초 유지
            txtTitle.CrossFadeColor(colors[(colorIndex++)%colors.Length], 2.5f, true, false);
        }      
    }

    public void IngameScene()   //화면 터치시 이벤트트리거를 통한 씬이동 함수 호출
    {
        SoundManager.instance.PlaySFX(SoundManager.eEffectType.BUTTON);
        Debug.Log(" load Scene");
        Invoke("loadScene", 0.5f);
    }

    void loadScene()
    {
        SceneChangeManager.instance.StartChangeScene("InGame");
    }
  
}
