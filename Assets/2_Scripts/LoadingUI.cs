using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] Text txtTip;
    [SerializeField] Text loadingTxt;
    [SerializeField] string[] tipMsgs;
  

    float loadingTxtTimeCheck = 0;
   

    // Update is called once per frame
    void Update()
    {        
        loadingTxtProgress();       
    }
    public void OpenWindow()
    {
        int tipIndex = Random.Range(0, tipMsgs.Length);
        SettingProgress(0);
        SettingTip(tipMsgs[tipIndex]);
    }

    public void SettingProgress(float rate) //씬 이동시 로딩되는정도에 맞게 로딩바 최신화
    {
        loadingBar.value = rate;    
    }

    public void SettingTip(string tipmsg)
    {
        txtTip.text = tipmsg;
    }

    void loadingTxtProgress()   //로딩 이펙트, 로딩지속에 따라 ...반복 찍기
    {
        loadingTxtTimeCheck += Time.deltaTime;


        int cnt = ((int)loadingTxtTimeCheck) % 4;
        loadingTxt.text = "Loading";
        for (int i = 0; i < cnt; i++)
        {
            loadingTxt.text += ".";
        }
    }

    
}
