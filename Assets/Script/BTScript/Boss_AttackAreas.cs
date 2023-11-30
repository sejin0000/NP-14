using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_AttackAreas : MonoBehaviour
{
    private BossAI_Dragon owner;
    private void Awake()
    {
        owner = GetComponentInParent<BossAI_Dragon>();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //지금 플레이어 들어왔어요
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatHandler player = collision.gameObject.GetComponent<PlayerStatHandler>();

            //TODO 플레이어가 죽어있는 경우도 확인[나중에 바꾸셈]
            if (owner.inToAreaPlayers != null)
            {
                // 리스트에 플레이어 추가
                owner.inToAreaPlayers.Add(player);
                Debug.Log($"리스트 개수{owner.inToAreaPlayers.Count} 개");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //지금 플레이어 나갔어요
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatHandler player = collision.gameObject.GetComponent<PlayerStatHandler>();
            if (owner.inToAreaPlayers != null)
            {
                owner.inToAreaPlayers.Remove(player);
                Debug.Log($"리스트 개수{owner.inToAreaPlayers.Count} 개");
            }
        }
    }


    private void OnDisable()
    {
        owner.inToAreaPlayers.Clear();
        Debug.Log($"리스크 클리어 됨 리스트 개수{owner.inToAreaPlayers.Count} 개");
    }
}
