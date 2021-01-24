using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour  //인게임중 일시정지 효과, timescale조정
{
    [SerializeField] GameObject _pauseMenu;

    public void SetPause()
    {
        _pauseMenu.SetActive(true);
        SoundManager.instance.StopBGM();
        Time.timeScale = 0;
    }

    public void ClosePause()
    {
        _pauseMenu.SetActive(false);
        SoundManager.instance.PlayBGM(1);
        Time.timeScale = 1;
    }
}
