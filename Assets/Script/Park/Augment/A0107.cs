using Photon.Pun;
using UnityEngine;

public class A0107 : MonoBehaviourPun
{
    private PlayerStatHandler playerStat;
    public float power;
    public float oldpower;
    bool Ismove;
    float powerTime = 0f;
    private void Awake()
    {
        if (photonView.IsMine)//알맞은 타이밍 //가만히 있는 시간에 비례하여 공업
        {
            playerStat = GetComponent<PlayerStatHandler>();

            playerStat.MoveStartEvent += MoveStartEvent;
            playerStat.MoveEndEvent += MoveEndEvent;
            power = 0;
            oldpower = 0;
            powerTime = 0f;
            Ismove =false;
        }
    }
    private void Update()
    {
        if (!Ismove && photonView.IsMine) 
        {
            playerStat.ATK.added += (Time.deltaTime) * 1f;
            power += Time.deltaTime * 1f;
            powerTime += Time.deltaTime;

        }

    }

    // Update is called once per frame
    void MoveStartEvent()
    {
        playerStat.ATK.added -= power;
        power = 0;
        Ismove = true;
    }
    void MoveEndEvent() 
    {
        Ismove=false;
        powerTime = 0f;
    }
}
