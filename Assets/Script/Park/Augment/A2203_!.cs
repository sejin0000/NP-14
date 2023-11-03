using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2203_1 : MonoBehaviour
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    GameObject Prefabs;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStat = GetComponent<PlayerStatHandler>();
    }
    private void Start()
    {
        controller.OnEndRollEvent +=   MakeHeal;
        Prefabs = Resources.Load<GameObject>("A2203");
    }

    // Update is called once per frame
    void MakeHeal()
    {
        GameObject fire = Instantiate(Prefabs);
        //fire.transform.SetParent(player.transform);
        A2203 a2203 = fire.GetComponent<A2203>();
        fire.transform.localPosition= playerStat.gameObject.transform.localPosition;
        a2203.Init(playerStat);
    }
}
