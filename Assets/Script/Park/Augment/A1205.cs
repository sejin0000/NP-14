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
    public GameObject skillCheckPrefab;
    public SkillCheckMaster skillCheckobj;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            playerInput = GetComponent<PlayerInput>();
            tempPower = 0;
            controller.OnStartSkillEvent += SkillCheck; // �߿��Ѻκ�
            skillCheckobj = Instantiate<SkillCheckMaster>(skillCheckobj);
            skillCheckobj.Init(this);
            skillCheckPrefab.SetActive(false);
        }
    }
    // Update is called once per frame
    void SkillCheck()
    {
        playerInput.actions.FindAction("Attack").Disable();
        skillCheckPrefab.SetActive(true);
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
        skillCheckPrefab.SetActive(false);
    }
}