using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1206 : MonoBehaviourPun
{
    [SerializeField] private float convertCoeff;        // ��ȯ ���
    [SerializeField] public float healTotal;            // ��ü ȸ���� HP

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
        Debug.Log($"���� ���� : {healTotal}");
    }    

    private void ConvertHealToATK(float healed, int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        pv.GetComponent<PlayerStatHandler>().ATK.added += (healed * convertCoeff);//�̽����ڵ鷯�� �ᱹ �ڱ� �����ڵ鷯�ƴ�?
        savepower += (healed * convertCoeff);
        Debug.Log($"�߰��� ���ݷ� : {healed * convertCoeff}");
    }
    private void resetAtk() 
    {
        statHandler.ATK.added -= savepower;
        savepower = 0;
    }
}
