using Photon.Pun;

public class A1302 : MonoBehaviourPun
{

    private WeaponSystem weaponSystem;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            weaponSystem = GetComponent<WeaponSystem>();
            if (GameManager.Instance != null) 
            {
                GameManager.Instance.OnStageStartEvent += reloaing;
                GameManager.Instance.OnBossStageStartEvent += reloaing;
            }
        }
    }
    // Update is called once per frame
    void reloaing()
    {
        weaponSystem.canresurrection = true;
    }
}
