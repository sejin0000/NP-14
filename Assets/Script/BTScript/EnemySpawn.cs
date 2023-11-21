using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviourPun
{
    public void Spawn(string _name)
    {
        string testEnemy = $"Prefabs/Enemy/{_name}";
        PhotonNetwork.Instantiate(testEnemy, transform.position, Quaternion.identity);
    }
}
