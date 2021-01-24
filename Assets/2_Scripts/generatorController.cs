using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class generatorController : MonoBehaviour        //벌레 생성
{
    [SerializeField] GameObject[] _prefabBugs;          //두 종류
    GameObject BugOBJ=null;                             //둘 중 하나 참조해서 생성되는 본체
    [SerializeField] Transform bugsRoot;                //생성 위치

    public static int uniqueBugCount = 2;               //빠른 벌레 생성 수

    public void startSpawn(int startTime,int repeatTime)    //생성 코루틴
    {
        InvokeRepeating("Respawn", startTime, repeatTime);
    }

    public void StopSpawn()
    {
        CancelInvoke("Respawn");
        
    }

    void Respawn()  //벌레들 생성 위치와 방향 설정
    {
        //생성 위치를 카메라화면상에 맞게 설정
        float screenH = Camera.main.orthographicSize;
        float screenW = screenH * Camera.main.aspect; //종횡비 곱해서 세로값 맞추기
        float angle = 0;        //생성시 방향
        Vector3 pos = Vector3.zero;//생성시 위치
        
       

        List<int> uniqueBugAppearPos = new List<int>();
        //중복되지않게 빠른 벌레 생성 위치 랜덤하게 뽑기
        for (int i = 0; i < uniqueBugCount; i++)
        {
            int temp = Random.Range(0, 8);
            while(uniqueBugAppearPos.Contains(temp))
                temp = Random.Range(0, 8);
            uniqueBugAppearPos.Add(temp);
        }
              

        for (int i = 0; i < 8; i++)
        {
            //해당 위치에 맞는 벌레 대입
            for (int j = 0; j < uniqueBugAppearPos.Count; j++)
            {
                if (i == uniqueBugAppearPos[j])
                {
                    BugOBJ = _prefabBugs[1];
                    break;
                }
                else
                    BugOBJ = _prefabBugs[0];
            }
            if (i == 0 || i == 1)
            {
                pos.Set(-screenW , UnityEngine.Random.Range(-screenH,screenH) , 0);
                angle = -90;
            }
            else if (i == 2 || i == 3)
            {
                pos.Set(screenW, UnityEngine.Random.Range(-screenH, screenH), 0);
                angle = 90;
            }
            else if (i == 4 || i == 5)
            {
                pos.Set(UnityEngine.Random.Range(-screenW, screenW),screenH, 0);
                angle = 180;

            }
            else if (i == 6 || i == 7)
            {
                pos.Set(UnityEngine.Random.Range(-screenW, screenW),-screenH, 0);
                angle = 0;

            }

            if (InGamManager.isStageSelected)
            {
                GameObject go = Instantiate(BugOBJ, bugsRoot);
                go.transform.position = pos;
                go.transform.Rotate(0, 0, angle);
            }
        }

       

    }
}
