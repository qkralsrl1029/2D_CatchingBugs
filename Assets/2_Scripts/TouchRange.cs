using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRange : MonoBehaviour
{
    static float touchRange = 10f;
    [SerializeField] LayerMask targetMask;
    [SerializeField] GameObject touchEffect;

    Camera Camera;


    public static void SetRadius(float radius)  //터치 범위 설정, 타이틀 화면에서 호출
    {
        touchRange = radius;
    }

    private void Start()
    {
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (InGamManager.isPlaying)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SoundManager.instance.PlaySFX(SoundManager.eEffectType.KILL);
                Vector3 mousePos = Input.mousePosition;
                mousePos = Camera.ScreenToWorldPoint(mousePos);     //마우스 위치를 받아와서 화면에 맞게 변환

                Vector3 targetPos = new Vector3(mousePos.x, mousePos.y, 0); 

                var _effect = Instantiate(touchEffect, targetPos, Quaternion.identity); //손 모양 이펙트
                _effect.transform.localScale = new Vector3(touchRange, touchRange, 0);
                Destroy(_effect, 0.5f);


                //벌레 레이어마스크만 검출, 범위 내 벌레들 배열에 저장
                Collider2D[] _target = Physics2D.OverlapCircleAll(targetPos,touchRange,targetMask );
                for (int i = 0; i < _target.Length; i++)
                {
                    GameObject temp = _target[i].gameObject;
                    if(temp.transform.tag=="bug")
                    {
                        Debug.Log("bug observed");
                        temp.GetComponent<BugControl>().getTouched();   //해당 벌레의 피격 함수 실행
                    }
                }
            }
        }
    }
}
