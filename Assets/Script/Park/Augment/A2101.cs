using Photon.Pun;
public class A2101 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnSkillEvent += AtkSpeedUp;
        }
    }
    // Update is called once per frame
    void AtkSpeedUp()
    {
        playerStat.AtkSpeed.added += 0.02f;
    }
}
