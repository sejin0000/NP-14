using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1206 : MonoBehaviour
{
    [SerializeField] private float convertCoeff;        // ��ȯ ���
    [SerializeField] public float healTotal;            // ��ü ȸ���� HP

    private CollisionController controller;
    private PlayerStatHandler statHandler;

    private void Awake()
    {
        controller = GetComponent<CollisionController>();
        controller.OnHealedEvent += CalculateHealedAmount;
        controller.OnHealedEvent += ConvertHealToATK;
        convertCoeff = 0.4f;

        foreach (var target in TestGameManager.Instance.playerInfoDictionary.Values)
        {
            target.gameObject.GetComponent<CollisionController>().CanSupport = true;
        }

        statHandler = GetComponent<PlayerStatHandler>();
    }

    private void CalculateHealedAmount(float healed)
    {
        healTotal += healed;
        Debug.Log($"���� ���� : {healTotal}");
    }    

    private void ConvertHealToATK(float healed)
    {
        statHandler.ATK.added += (healed * convertCoeff);
        Debug.Log($"�߰��� ���ݷ� : {healed * convertCoeff}");
    }
}
