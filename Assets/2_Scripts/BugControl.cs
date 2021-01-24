using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BugControl : MonoBehaviour //벌레 움직임 관련 스크립트
{
    Animator anim;
    bool isDead = false;
    [SerializeField] float moveSpeed;
    [SerializeField] float changeDirTime;
    [SerializeField] int KillScore;
    float[] randomDegree = { -45, 0, 45 };

    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("ChangeDir", 2, changeDirTime);     //코루틴 실행, 2초마다 방향 전환
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) 
            return;
       
        this.transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);  //생성 후 계속 전진, 방향만 바꿔줌       
    }
    
    void ChangeDir()
    {      
        //랜덤하게 각도를 부여
        int rnd = Random.Range(0, 3);
        transform.Rotate(0, 0, randomDegree[rnd]);
    }

   
   
    public void getTouched()    //터치시 일정범위내로 벌레 검출, 컬라이더 내에 존재시 호출
    {
       
        SoundManager.instance.PlaySFX(SoundManager.eEffectType.KILL);   //한번에 많은 벌레가 죽을수록 더 큰 소리가 남
        CancelInvoke("ChangeDir");      //방향전환 코루틴 종료
        if (!InGamManager._instance.isEnd)    
        {
            UIScript.instance.BugKill(KillScore);    //킬 카운트 증가
            isDead = true;
            GetComponent<CircleCollider2D>().enabled = false;   //컬라이더 제거해서 중복실행 방지
            anim.SetBool("isDead", isDead); //죽는 애니메이션 실행
        }
        //CancelInvoke();

        Destroy(gameObject, 3);         //일정 시간 후 오브젝트 삭제
    }

    public bool _isDead()
    { return isDead; }
}
