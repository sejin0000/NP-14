using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] enemys;

    public void Spawn(string name)
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i].GetComponent<EnemyAI>().enemySO.enemyName == name)
            {
                Instantiate(enemys[i]);
            }
        }   
    }
}
