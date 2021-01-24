using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushTxt : MonoBehaviour    //타이틀 창에 텍스트 깜빡이는 이펙트 효과
{
    [SerializeField] Text pushTxt;
    float timeCheck = 0;
    bool isAppear = true;

    // Update is called once per frame
    void Update()
    {
        timeCheck += Time.deltaTime;

        if(timeCheck>0.5f)
        {
            timeCheck = 0;
            pushTxt.gameObject.SetActive(!isAppear);
            isAppear = !isAppear;
        }
    }
}
