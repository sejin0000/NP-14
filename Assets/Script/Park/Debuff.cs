using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Debuff : MonoBehaviourPun
{
    public static Debuff Instance;

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }
    public void GiveFire(GameObject gameObject , float totalpower) 
    {
        float power = totalpower * 0.2f;

        if (gameObject.tag == "Enemy" && gameObject.GetComponent<EnemyAI>().CanFire) 
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            photonView.RPC("FireGive", RpcTarget.All, power, viewID);
        }
    }

    [PunRPC]
    public void FireGive(float power,int viewID)
    {
        StartCoroutine(Fire(power, viewID));
    }

    static IEnumerator Fire(float damege, int viewID)
    {
        int endtime = 1;
        PhotonView photonView = PhotonView.Find(viewID);
        GameObject targetPlayer = photonView.gameObject;
        EnemyAI a = targetPlayer.GetComponent<EnemyAI>();
        if (a.CanFire) 
        {
            a.CanFire = false;
            for (int i = 0; i < 5; ++i)
            {
                a.DecreaseHP(damege);
                yield return new WaitForSeconds(endtime);
            }
            a.CanFire = true;
        }

    }
    public void GiveIce(GameObject gameObject)
    {

        if (gameObject.tag == "Enemy" && gameObject.GetComponent<EnemyAI>().CanIce)
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            photonView.RPC("IceGive", RpcTarget.All, viewID);
        }
    }

    [PunRPC]
    public void IceGive(int viewID)
    {
        StartCoroutine("Ice",viewID);
    }

    static IEnumerator Ice(int viewID)
    {
        int endtime = 5;
        PhotonView photonView = PhotonView.Find(viewID);
        GameObject targetPlayer = photonView.gameObject;
        EnemyAI a = targetPlayer.GetComponent<EnemyAI>();
        NavMeshAgent nav = targetPlayer.GetComponent<NavMeshAgent>();
        nav.speed = nav.speed * 0.8f;
        if (a.CanIce) 
        {
            a.CanIce=false;
            a.SpeedCoefficient = 0.8f;
            yield return new WaitForSeconds(endtime);
            a.SpeedCoefficient = 1f;
            a.CanIce = true;
        }
    }

    public void GiveLowSteamPack(GameObject gameObject)
    {
        if (gameObject.tag == "Player" )
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            photonView.RPC("LowSteamPackGive", RpcTarget.All, viewID);
        }
    }

    [PunRPC]
    public void LowSteamPackGive(int viewID)
    {
        StartCoroutine("LowSteamPack", viewID);
    }

    static IEnumerator LowSteamPack(int viewID)
    {
        int endtime = 3;
        PhotonView photonView = PhotonView.Find(viewID);
        PlayerStatHandler targetPlayer = photonView.gameObject.GetComponent<PlayerStatHandler>();
        if (targetPlayer.CanLowSteam) 
        {
            targetPlayer.CanLowSteam = false;
            targetPlayer.AtkSpeed.added += 0.5f;
            targetPlayer.Speed.added += 0.5f;
            yield return new WaitForSeconds(endtime);
            targetPlayer.AtkSpeed.added -= 0.5f;
            targetPlayer.Speed.added -= 0.5f;
            targetPlayer.CanLowSteam = true;
        }
    }
    public void GiveTouchSpeed(GameObject gameObject)
    {
        if (gameObject.tag == "Player" && gameObject.GetComponent<PlayerStatHandler>().CanSpeedBuff)
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            photonView.RPC("GiveSpeed", RpcTarget.All, viewID);
        }
    }

    [PunRPC]
    public void GiveSpeed(int viewID)
    {
        StartCoroutine("LowSpeed", viewID);
    }

    private IEnumerator LowSpeed(int viewID)
    {
        Debug.Log("LowSpeed 코루틴 돌아가는중 ....");        
        int endtime = 3;
        PhotonView photonView = PhotonView.Find(viewID);
        PlayerStatHandler targetPlayer = photonView.gameObject.GetComponent<PlayerStatHandler>();
        if (targetPlayer.CanSpeedBuff) 
        {
            Debug.Log("스피드.... ");
            targetPlayer.CanSpeedBuff = false;
            targetPlayer.Speed.added += 3f;
            Debug.Log($"현재 속도 1: {targetPlayer.Speed.total}");
            yield return new WaitForSeconds(endtime);
            targetPlayer.Speed.added -= 3f;
            targetPlayer.CanSpeedBuff = true;
            Debug.Log($"현재 속도 2: {targetPlayer.Speed.total}");
        }
    }

}
