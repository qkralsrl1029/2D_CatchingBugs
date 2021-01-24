using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TitleScreecScript : MonoBehaviour
{
    [SerializeField] GameObject optionPage;

    [SerializeField] Slider radiusSlider;
    [SerializeField] Slider soundSlider;

    [SerializeField] Image circleImage;
    float tempScale = 0.8f;
 

    // Update is called once per frame
    void Update()   //타이틀 화면에서 볼륨과 터치범위가 바뀔때마다 최신화
    {
        setRad();
        setVol();
        
    }
    
    void setRad()
    {
        tempScale = 0.8f + (1.2f * radiusSlider.value);
        circleImage.transform.localScale = new Vector3(tempScale, tempScale, tempScale);
        TouchRange.SetRadius(0.5f + 2*radiusSlider.value);
    }

    void setVol()
    {
        SoundManager.instance.SetBGMVolume(soundSlider.value);
    }

    public void SetOption()
    {
        optionPage.SetActive(true);
    }

    public void CloseOption()
    {
        optionPage.SetActive(false);
    }
}
