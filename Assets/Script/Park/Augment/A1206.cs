using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1206 : MonoBehaviour
{
    [SerializeField] private float convertCoeff;        // 전환 계수
    [SerializeField] public float healTotal;            // 전체 회복된 HP

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
        Debug.Log($"누적 힐량 : {healTotal}");
    }    

    private void ConvertHealToATK(float healed)
    {
        statHandler.ATK.added += (healed * convertCoeff);
        Debug.Log($"추가된 공격력 : {healed * convertCoeff}");
    }
}
