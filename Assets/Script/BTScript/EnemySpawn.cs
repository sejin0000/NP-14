using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviourPun
{
    public void Spawn()
    {
        string testEnemy = "Pefabs/Enemy/Test_Enemy";
        PhotonNetwork.Instantiate(testEnemy, transform.position, Quaternion.identity);
    }
}
