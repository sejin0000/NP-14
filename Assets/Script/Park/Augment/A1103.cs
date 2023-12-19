using Photon.Pun;
using UnityEngine;

public class A1103 : MonoBehaviourPun
{
    [SerializeField] private float convertCoeff;        // 전환 계수
    [SerializeField] public float healTotal;            // 전체 회복된 HP

    private CollisionController controller;
    private PlayerStatHandler statHandler;

    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<CollisionController>();
            controller.OnHealedEvent += ConvertHealToHeal;
            convertCoeff = 0.3f;
            statHandler = GetComponent<PlayerStatHandler>();
        }
    }
    private void ConvertHealToHeal(float healed, int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        pv.GetComponent<PlayerStatHandler>().HPadd(healed * convertCoeff);
    }
}
