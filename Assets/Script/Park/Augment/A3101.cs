using Photon.Pun;
using UnityEngine;

public class A3101 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;

    public float heal=12f;
    public float healTime = 5f;
    float time = 0f;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();

            playerStat.HitEvent += restartTime; // 중요한부분
        }
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            time += Time.deltaTime;
            if (time >= healTime)
            {
                StayHeal();
                time = 0f;
            }
        }
    }
    // Update is called once per frame
    void StayHeal()
    {
        if (photonView.IsMine) 
        {
            playerStat.HPadd(heal);
        }
    }
    void restartTime() 
    {
        time = 0;
    }
}
