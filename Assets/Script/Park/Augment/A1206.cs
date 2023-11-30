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

    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<CollisionController>();
            controller.OnHealedEvent += CalculateHealedAmount;
            controller.OnHealedEvent += ConvertHealToATK;

            statHandler = GetComponent<PlayerStatHandler>();
            convertCoeff = 0.4f;
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
        pv.GetComponent<PlayerStatHandler>().ATK.added += (healed * convertCoeff);
        Debug.Log($"�߰��� ���ݷ� : {healed * convertCoeff}");
    }
}
