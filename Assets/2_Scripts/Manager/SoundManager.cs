using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] AudioClip[] bgmClips;
    AudioSource _bgmPlayer;
   

    [SerializeField] AudioClip[] effectClips;
    [SerializeField] GameObject sfxPlayer;
    List<GameObject> sfxPlayers = new List<GameObject>();
    float sfxVolume = 1;


    public enum eEffectType
    {
        KILL=0,
        BUTTON=1,
        TOUCH=2,
        COUNT,
        NEWRECORD,
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _bgmPlayer = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //사용되지않는 효과음 매 프레임 검사 후 삭제
        for (int i = 0; i < sfxPlayers.Count; i++)
        {
            if (sfxPlayers[i] != null)
            {
                if (!sfxPlayers[i].GetComponent<AudioSource>().isPlaying)
                {
                    Destroy(sfxPlayers[i]);
                    Debug.Log("delete sfx");
                }
            }
        }
    }


    public void PlayBGM(int index,bool loop=true)   //브금 플레이
    {
        _bgmPlayer.clip = bgmClips[index];       //미리 저장해둔 클립들중에서 인자로 넘어온 인덷스의 클립 설정
        _bgmPlayer.loop = loop;                  //루프 유무 설정
        _bgmPlayer.Play();                       //플레이
    }
    
    public void SetBGMVolume(float bgmVol)
    {
        _bgmPlayer.volume = bgmVol;
    }

    public void StopBGM()
    {
        _bgmPlayer.Stop();
    }

    

    public void PlaySFX(eEffectType type, bool loop = false)
    {
        GameObject _sfxPlayer = Instantiate(sfxPlayer); //오디오소스를 프리팹으로 생성시켜 재생
        _sfxPlayer.GetComponent<AudioSource>().loop = loop;
        _sfxPlayer.GetComponent<AudioSource>().clip = effectClips[(int)type];
        _sfxPlayer.GetComponent<AudioSource>().volume = sfxVolume;
        _sfxPlayer.GetComponent<AudioSource>().Play();
        sfxPlayers.Add(_sfxPlayer); //생성된 효과음은 리스트에 넣어서 일괄 관리
        
    }

    public void StopAllSFX()
    {
        for (int i = 0; i < sfxPlayers.Count; i++)
        {
            if (sfxPlayers[i] != null)             
                Destroy(sfxPlayers[i]);            
        }
    }

    public float _sfxVol        //볼륨 설정
    {
        get { return sfxVolume; }
        set { sfxVolume = value; }
    }
}
