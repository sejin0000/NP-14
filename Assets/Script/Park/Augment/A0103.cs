using Photon.Pun;
public class A0103 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    float nowCoolGAM;
    float oldCoolGAM;
    private void Awake()//���� ź���� ++ = �����ð�����
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowCoolGAM = playerStat.BulletSpread.total * 0.2f;
            playerStat.ReloadCoolTime.added -= nowCoolGAM;
            oldCoolGAM = nowCoolGAM;
            GameManager.Instance.OnStageStartEvent += SetCool;
            GameManager.Instance.OnBossStageStartEvent += SetCool;
        }
    }
    // Update is called once per frame
    void SetCool()
    {
        playerStat.ReloadCoolTime.added += oldCoolGAM;
        nowCoolGAM = playerStat.BulletSpread.total * 0.2f;
        playerStat.ReloadCoolTime.added -= nowCoolGAM;
        oldCoolGAM = nowCoolGAM;
        
    }
}
