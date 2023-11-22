using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class A1205 : MonoBehaviourPun//스킬 사용시 스킬체크를 하여 성공시 데미지/힐량을 증가 시키지만 실패시 데미지/힐량을 감소 시킵니다.
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
    ///  무조건 f키로 한다 진짜로 진짜로 진짜
    ///  아니 넌 틀렸어 인풋시스템을 한개 더만든다 
    /// </summary>
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            playerInput = GetComponent<PlayerInput>();
            tempPower = 0;
            controller.OnStartSkillEvent += SkillCheck; // 중요한부분
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
