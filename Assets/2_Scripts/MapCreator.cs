using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)  //해당 컬리이더 영역에서 벗어나는 벌레 삭제
    {
        if (collision.tag == "bug"&&!collision.GetComponent<BugControl>()._isDead())
            Destroy(collision.gameObject);
    }
    
}
