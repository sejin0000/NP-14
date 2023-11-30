using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1206 : MonoBehaviourPun
{
    [SerializeField] private float convertCoeff;        // 전환 계수
    [SerializeField] public float healTotal;            // 전체 회복된 HP

    private CollisionController controller;
    private PlayerStatHandler statHandler;

    float savepower;
    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<CollisionController>();
            controller.OnHealedEvent += CalculateHealedAmount;
            controller.OnHealedEvent += ConvertHealToATK;
            savepower = 0;
            statHandler = GetComponent<PlayerStatHandler>();
            convertCoeff = 0.4f;
            if (GameManager.Instance != null) 
            {
                GameManager.Instance.OnStageStartEvent +=resetAtk;
                GameManager.Instance.OnBossStageStartEvent += resetAtk;
            }
        }
    }

    private void CalculateHealedAmount(float healed, int viewID)
    {
        healTotal += healed;
        Debug.Log($"누적 힐량 : {healTotal}");
    }    

    private void ConvertHealToATK(float healed, int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        pv.GetComponent<PlayerStatHandler>().ATK.added += (healed * convertCoeff);//이스탯핸들러가 결국 자기 스탯핸들러아님?
        savepower += (healed * convertCoeff);
        Debug.Log($"추가된 공격력 : {healed * convertCoeff}");
    }
    private void resetAtk() 
    {
        statHandler.ATK.added -= savepower;
        savepower = 0;
    }
}
