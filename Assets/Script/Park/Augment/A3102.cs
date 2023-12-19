using Photon.Pun;

public class A3102 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnSkillEvent += SkillHpUp;
        }

    }
    void SkillHpUp()
    {
        playerStat.HP.added += 2f;
    }
}
