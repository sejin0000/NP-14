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
        //���� �÷��̾� ���Ծ��
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatHandler player = collision.gameObject.GetComponent<PlayerStatHandler>();

            //TODO �÷��̾ �׾��ִ� ��쵵 Ȯ��[���߿� �ٲټ�]
            if (owner.inToAreaPlayers != null)
            {
                // ����Ʈ�� �÷��̾� �߰�
                owner.inToAreaPlayers.Add(player);
                Debug.Log($"����Ʈ ����{owner.inToAreaPlayers.Count} ��");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //���� �÷��̾� �������
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatHandler player = collision.gameObject.GetComponent<PlayerStatHandler>();
            if (owner.inToAreaPlayers != null)
            {
                owner.inToAreaPlayers.Remove(player);
                Debug.Log($"����Ʈ ����{owner.inToAreaPlayers.Count} ��");
            }
        }
    }


    private void OnDisable()
    {
        owner.inToAreaPlayers.Clear();
        Debug.Log($"����ũ Ŭ���� �� ����Ʈ ����{owner.inToAreaPlayers.Count} ��");
    }
}
