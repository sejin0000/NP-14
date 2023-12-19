using Photon.Pun;

public class A0108 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();

            playerStat.KillCatchEvent += DrainPower; // 중요한부분
        }
    }
    // Update is called once per frame
    void DrainPower()
    {
        if (playerStat.CanSpeedBuff) 
        {
            Debuff.Instance.GiveTouchSpeed(this.gameObject);
        }
    }
}
