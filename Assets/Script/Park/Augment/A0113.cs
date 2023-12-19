using Photon.Pun;

public class A0113 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;

    private int nowgold;
    private float nowpower;
    private float oldpower;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowgold = GameManager.Instance.TeamGold;
            oldpower = 0;
            GameManager.Instance.ChangeGoldEvent += setgold; 

        }
    }
    // Update is called once per frame
    void setgold()
    {
        nowpower = nowgold * 0.05f;
        playerStat.ATK.added += nowpower; 
        playerStat.ATK.added -= oldpower; 
        oldpower = nowpower;
    }
}
