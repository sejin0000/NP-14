using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Debuff : MonoBehaviourPun
{
    public static void GiveFire(GameObject gameObject , float totalpower) 
    {
        float power = totalpower * 0.2f;

        if (gameObject.tag == "Enemy") 
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
        for (int i = 0; i < 5; ++i) 
        {
            a.DecreaseHP(damege);
            yield return endtime;
        }
    }
    public static void GiveIce(GameObject gameObject)
    {

        if (gameObject.tag == "Enemy")
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
        a.SpeedCoefficient = 0.8f;
        yield return endtime;
        a.SpeedCoefficient = 1f;

    }

    public static void GiveLowSteamPack(GameObject gameObject)
    {
        if (gameObject.tag == "Player")
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
        int endtime = 5;
        PhotonView photonView = PhotonView.Find(viewID);
        PlayerStatHandler targetPlayer = photonView.gameObject.GetComponent<PlayerStatHandler>();
        targetPlayer.AtkSpeed.added += 0.5f;
        targetPlayer.Speed.added += 0.5f;
        yield return endtime;
        targetPlayer.AtkSpeed.added -= 0.5f;
        targetPlayer.Speed.added -= 0.5f;

    }
    public static void GiveTouchSpeed(GameObject gameObject)
    {
        if (gameObject.tag == "Player")
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
        int endtime = 2;
        PhotonView photonView = PhotonView.Find(viewID);
        PlayerStatHandler targetPlayer = photonView.gameObject.GetComponent<PlayerStatHandler>();

        targetPlayer.Speed.added += 0.5f;
        yield return endtime;
        targetPlayer.Speed.added -= 0.5f;

    }

}
