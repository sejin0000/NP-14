using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Debuff : MonoBehaviourPun
{
    public static void GiveFire(GameObject gameObject , float totalpower) 
    {
        float power = totalpower * 0.2f;

        if (gameObject.tag == "Enemy" && gameObject.GetComponent<EnemyAI>().CanFire) 
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            pv.RPC("FireGive", RpcTarget.All, power, viewID);
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
                yield return endtime;
            }
            a.CanFire = true;
        }

    }
    public static void GiveIce(GameObject gameObject)
    {

        if (gameObject.tag == "Enemy" && gameObject.GetComponent<EnemyAI>().CanIce)
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            pv.RPC("IceGive", RpcTarget.All, viewID);
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
            yield return endtime;
            a.SpeedCoefficient = 1f;
            a.CanIce = true;
        }
    }

    public static void GiveLowSteamPack(GameObject gameObject)
    {
        if (gameObject.tag == "Player" )
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            pv.RPC("LowSteamPackGive", RpcTarget.All, viewID);
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
            yield return endtime;
            targetPlayer.AtkSpeed.added -= 0.5f;
            targetPlayer.Speed.added -= 0.5f;
            targetPlayer.CanLowSteam = true;
        }
    }
    public static void GiveTouchSpeed(GameObject gameObject)
    {
        if (gameObject.tag == "Player" && gameObject.GetComponent<PlayerStatHandler>().CanSpeedBuff)
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            pv.RPC("GiveSpeed", RpcTarget.All, viewID);
        }
    }

    [PunRPC]
    public void GiveSpeed(int viewID)
    {
        StartCoroutine("LowSpeed", viewID);
    }

    static IEnumerator LowSpeed(int viewID)
    {
        int endtime = 1;
        PhotonView photonView = PhotonView.Find(viewID);
        PlayerStatHandler targetPlayer = photonView.gameObject.GetComponent<PlayerStatHandler>();
        if (targetPlayer.CanSpeedBuff) 
        {
            targetPlayer.CanSpeedBuff = false;
            targetPlayer.Speed.added += 3f;
            yield return endtime;
            targetPlayer.Speed.added -= 3f;
            targetPlayer.CanSpeedBuff = true;
        }
    }

}
