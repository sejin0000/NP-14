using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1103 : MonoBehaviour
{
    [SerializeField] private float convertCoeff;        // ��ȯ ���
    [SerializeField] public float healTotal;            // ��ü ȸ���� HP

    private CollisionController controller;
    private PlayerStatHandler statHandler;

    private void Awake()
    {
        controller = GetComponent<CollisionController>();
        controller.OnHealedEvent += ConvertHealToHeal;
        convertCoeff = 0.3f;

        foreach (var target in TestGameManager.Instance.playerInfoDictionary.Values)
        {
            target.gameObject.GetComponent<CollisionController>().CanPayBack = true;
        }

        statHandler = GetComponent<PlayerStatHandler>();
    }
    private void ConvertHealToHeal(float healed)
    {
        statHandler.HPadd(healed * convertCoeff);
        Debug.Log($"A1103 ȸ���� HP : {healed * convertCoeff}");
    }
}
