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

    private void OnEnable()
    {
        owner.inToAreaPlayers.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���� �÷��̾� ���Ծ��
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatHandler player = collision.gameObject.GetComponent<PlayerStatHandler>();

            //TODO �÷��̾ �׾��ִ� ��쵵 Ȯ��[���߿� �ٲټ�]
            if (player != null)
            {
                // ����Ʈ�� �÷��̾� �߰�
                owner.inToAreaPlayers.Add(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //���� �÷��̾� �������
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatHandler player = collision.gameObject.GetComponent<PlayerStatHandler>();
            if (player != null)
            {
                owner.inToAreaPlayers.Remove(player);
            }
        }
    }


    private void OnDisable()
    {
        owner.inToAreaPlayers.Clear();
    }
}
