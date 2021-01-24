using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour   //결과창
{
    //결과창
    [SerializeField] GameObject resultWindow;
    //터치수,킬수,해당 스테이지 최고점수
    [SerializeField] Text killResult;
    [SerializeField] Text touchResult;
    [SerializeField] Text maxResult;
    //각 버튼들
    [SerializeField] Button resetButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button exitButton;
    //최고점수 표시 이펙트
    [SerializeField] GameObject newRecordEffect;

    //해당 게임점수가 최고점수보다 높은지 검사, 인게임매니저에서 참조
    public static bool isHigherThanTop = false;
    int totalKill;
    int totalTouch;
    int maxScore;
    private void OnEnable() //활성화될때마다
    {
        //점수 받아오고
        totalKill = UIScript.instance.getScore();
        totalTouch = UIScript.instance.getTotal();
        maxScore = UIScript.instance.getMax();

        //최고점수면 이펙트 표시
        if (isHigherThanTop)
        {
            newRecordEffect.SetActive(true);
            SoundManager.instance.PlaySFX(SoundManager.eEffectType.NEWRECORD);
            isHigherThanTop = false;
        }
        else
            newRecordEffect.SetActive(false);
    }
   
    public void showResult()
    {
        touchResult.gameObject.SetActive(false);
        resetButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        maxResult.gameObject.SetActive(false);
        resultWindow.SetActive(true);
        StartCoroutine(_showResult());
    }

    IEnumerator _showResult()
    {
        SoundManager.instance.PlaySFX(SoundManager.eEffectType.COUNT, true);
        //총 킬수가 될때까지 임시정수를 증가시키면서 결과창에 최신화
        for (int i = 0; i < totalKill+1; i++)
        {
            killResult.text = i.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        SoundManager.instance.StopAllSFX(); //점수올라가는 소리 종료

        //점수표시 끝나면 다른 점수들 보이게
        touchResult.text = totalTouch.ToString();
        touchResult.gameObject.SetActive(true);

        maxResult.gameObject.SetActive(true);
        maxResult.text = maxScore.ToString();

        //스코어 반영이 끝나면 재시작 버튼이 보이도록 설정
        yield return new WaitForSeconds(0.1f);
        resetButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
    }

    

    public void RESTART()
    {
        StopAllCoroutines();        //재시작 버튼이 눌리면 코루틴 종료

        //게임 초기화
        InGamManager.isStageSelected = false;
        InGamManager.isPlaying = false;
        InGamManager._instance.isEnd = false;
        UIScript.instance.ResetScore();
        SoundManager.instance.PlaySFX(SoundManager.eEffectType.BUTTON);
        //아예 씬을 다시 로드 해도됨


        //결과창 닫기
        this.gameObject.SetActive(false);

        SceneChangeManager.instance.StartChangeScene("InGame");
    }

    public void GoTitle()
    { 
        SoundManager.instance.PlaySFX(SoundManager.eEffectType.BUTTON);
        InGamManager.isPlaying = false;
        InGamManager.isStageSelected = false;
        SceneChangeManager.instance.StartTitleScene("TitleScene");
    }

    public void Exit()
    {
        SoundManager.instance.PlaySFX(SoundManager.eEffectType.BUTTON);
        InGamManager.isPlaying = false;
        InGamManager.isStageSelected = false;
        //전처리 구문, 빌드시에 실행
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;    //에디터 상에서의 종료
#else
        Application.Quit(); //어플리케이션 상태에서 종료
#endif
    }
}
