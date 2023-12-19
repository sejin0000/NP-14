using Photon.Pun;

public class A0222 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnRollEvent += RollingHeal;
        }
    }
    void RollingHeal()
    {
        playerStat.HPadd(playerStat.HP.total * 0.1f);
    }
}
