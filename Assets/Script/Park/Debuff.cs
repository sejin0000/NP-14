using Photon.Pun;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Debuff : MonoBehaviourPun
{
    public static Debuff Instance;

    public GameObject debuffFirePrefab;
    public GameObject debuffWaterPrefab;
    public GameObject debuffIcePrefab;
    public GameObject debuffHealPrefab;


    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }
    public void GiveFire(GameObject gameObject , float totalpower , int myPVID) 
    {
        float power = totalpower * 0.2f;

        if (gameObject.tag == "Enemy" && gameObject.GetComponent<EnemyAI>().CanFire) 
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            photonView.RPC("FireGive", RpcTarget.All, power, viewID,myPVID);
        }
    }

    [PunRPC]
    public void FireGive(float power,int viewID, int myPVID)
    {
        StartCoroutine(Fire(power, viewID, myPVID));
    }

    private IEnumerator Fire(float damege, int viewID, int myPVID)
    {
        int endtime = 1;
        PhotonView photonView = PhotonView.Find(viewID);
        GameObject targetPlayer = photonView.gameObject;
        EnemyAI enemy = targetPlayer.GetComponent<EnemyAI>();
        float finalDamege = (damege < enemy.enemySO.hp * 0.01f)? enemy.enemySO.hp * 0.005f : damege;

        if (enemy.CanFire) 
        {
            enemy.CanFire = false;

            GameObject particleIce = Instantiate(debuffFirePrefab);
            particleIce.transform.SetParent(enemy.gameObject.transform);
            particleIce.transform.localPosition= Vector3.zero;
            for (int i = 0; i < 5; ++i)
            {
                if (!enemy.isLive)
                {
                    yield return null;
                }
                photonView.RPC("DecreaseHPByObject", RpcTarget.Others, finalDamege, myPVID);
                yield return new WaitForSeconds(endtime);
            }
            enemy.CanFire = true;
        }

    }
    public void GiveWater(GameObject gameObject)
    {

        if (gameObject.tag == "Enemy" && gameObject.GetComponent<EnemyAI>().CanWater)
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            photonView.RPC("WaterGive", RpcTarget.All, viewID);
        }
    }

    [PunRPC]
    public void WaterGive(int viewID)
    {
        StartCoroutine("Water",viewID);
    }

    private IEnumerator Water(int viewID)
    {
        int endtime = 5;
        PhotonView photonView = PhotonView.Find(viewID);
        GameObject targetPlayer = photonView.gameObject;
        EnemyAI enemy = targetPlayer.GetComponent<EnemyAI>();
        NavMeshAgent nav = targetPlayer.GetComponent<NavMeshAgent>();
        nav.speed = nav.speed * 0.7f;
        if (enemy.CanWater) 
        {
            enemy.CanWater=false;
            enemy.SpeedCoefficient = 0.8f;
            GameObject particleIce = Instantiate(debuffWaterPrefab);
            particleIce.transform.SetParent(enemy.gameObject.transform);
            particleIce.transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(endtime);
            enemy.SpeedCoefficient = 1f;
            enemy.CanWater = true;
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

    private IEnumerator LowSteamPack(int viewID)
    {
        int endtime = 3;
        PhotonView photonView = PhotonView.Find(viewID);
        PlayerStatHandler targetPlayer = photonView.gameObject.GetComponent<PlayerStatHandler>();
        if (targetPlayer.CanLowSteam) 
        {
            targetPlayer.CanLowSteam = false;
            targetPlayer.AtkSpeed.added += 0.5f;
            targetPlayer.Speed.added += 0.5f;
            targetPlayer._DebuffControl.Init(PlayerDebuffControl.buffName.Speed, endtime);
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
            targetPlayer._DebuffControl.Init(PlayerDebuffControl.buffName.Speed, endtime);
            Debug.Log($"현재 속도 1: {targetPlayer.Speed.total}");
            yield return new WaitForSeconds(endtime);
            targetPlayer.Speed.added -= 3f;
            targetPlayer.CanSpeedBuff = true;
            Debug.Log($"현재 속도 2: {targetPlayer.Speed.total}");
        }
    }
    public void GiveAtkBuff(GameObject gameObject)
    {
        if (gameObject.tag == "Player" && gameObject.GetComponent<PlayerStatHandler>().CanAtkBuff)
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            photonView.RPC("GiveAtk", RpcTarget.All, viewID);
        }
    }

    [PunRPC]
    public void GiveAtk(int viewID)
    {
        StartCoroutine("GoodAtk", viewID);
    }

    private IEnumerator GoodAtk(int viewID)
    {
        Debug.Log("LowSpeed 코루틴 돌아가는중 ....");
        int endtime = 3;
        PhotonView photonView = PhotonView.Find(viewID);
        PlayerStatHandler targetPlayer = photonView.gameObject.GetComponent<PlayerStatHandler>();
        if (targetPlayer.CanAtkBuff)
        {
            Debug.Log("스피드.... ");
            targetPlayer.CanAtkBuff = false;
            targetPlayer.ATK.coefficient += 0.1f;
            Debug.Log($"현재 속도 1: {targetPlayer.Speed.total}");
            yield return new WaitForSeconds(endtime);
            targetPlayer.ATK.coefficient -= 0.1f;
            targetPlayer.CanAtkBuff = true;
            Debug.Log($"현재 속도 2: {targetPlayer.Speed.total}");
        }
    }

    public void GiveIce(GameObject gameObject)
    {

        if (gameObject.tag == "Enemy" && gameObject.GetComponent<EnemyAI>().CanIce)
        {
            PhotonView pv = gameObject.GetComponent<PhotonView>();
            int viewID = pv.ViewID;
            photonView.RPC("IceGive", RpcTarget.All, viewID);
            Debug.Log("얼음체크");
        }
    }
    [PunRPC]
    public void IceGive(int viewID)
    {
        StartCoroutine("Ice", viewID);
    }

    private IEnumerator Ice(int viewID)
    {
        float endtime = 1.5f;
        PhotonView photonView = PhotonView.Find(viewID);
        GameObject targetenemy = photonView.gameObject;
        EnemyAI enemy = targetenemy.GetComponent<EnemyAI>();
        if (enemy.CanIce)
        {
            Debug.Log("얼음체크");
            enemy.CanIce = false;
            GameObject particleIce = Instantiate(debuffIcePrefab);
            particleIce.transform.SetParent(enemy.gameObject.transform);
            particleIce.transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(endtime);
            enemy.CanIce = true;
        }
    }

}
