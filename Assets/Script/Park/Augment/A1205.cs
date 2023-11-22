using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class A1205 : MonoBehaviourPun//��ų ���� ��ųüũ�� �Ͽ� ������ ������/������ ���� ��Ű���� ���н� ������/������ ���� ��ŵ�ϴ�.
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private PlayerInput playerInput;

    private float tempPower;
    public float power;
    public GameObject skillCheckObj;
    private SkillCheckMaster skillCheckobj;
    //float superPower;

    /// <summary>
    ///  ������ fŰ�� �Ѵ� ��¥�� ��¥�� ��¥
    ///  �ƴ� �� Ʋ�Ⱦ� ��ǲ�ý����� �Ѱ� ������� 
    /// </summary>
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            playerInput = GetComponent<PlayerInput>();
            tempPower = 0;
            controller.OnStartSkillEvent += SkillCheck; // �߿��Ѻκ�
            Instantiate(skillCheckObj);
            skillCheckObj.SetActive(false);
        }
    }
    // Update is called once per frame
    void SkillCheck()
    {
        playerInput.actions.FindAction("Attack").Disable();
        skillCheck2();
    }
    void skillCheck2() 
    {
        skillCheckobj.Init(this);
        skillCheckObj.SetActive(true);
    }
    public void endCall(float power) 
    {
        playerInput.actions.FindAction("Attack").Enable();
        playerStat.ATK.coefficient -= tempPower;
        playerStat.ATK.coefficient += power;
        tempPower = power;
        Invoke("objActiveControl", 0.5f);
    }
    void objActiveControl() 
    {
        skillCheckObj.SetActive(false);
    }
}
