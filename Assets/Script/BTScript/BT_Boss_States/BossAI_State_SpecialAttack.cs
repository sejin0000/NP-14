using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_SpecialAttack : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;
    private Transform target;

    private float currentTime;         // �ð� ����
    int randomPattern = Random.Range(0, 2);
    public BossAI_State_SpecialAttack(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {
        currentTime = bossSO.atkDelay;
        SetStateColor();
        target = bossAI_Dragon.currentTarget; //��Ʈ���� ���� ������ Ÿ�� ����
    }

    public override Status Update()
    {
        //��ó���� �Ұ� -> ��� �÷��̾��� ��ġ�� �޾Ƽ�, ���� ������ �ǹ��� ������� Ȯ��
        //�����ٸ�? Ư�� ���� ���� ->�ִٸ�? ���� ��ȯ[�븻 �������� �ٷ� �Ѿ]
       

        for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
        {
            float bossHeadToPlayers = Vector2.Distance(bossAI_Dragon.PlayersTransform[i].position, bossAI_Dragon.bossHead.position);

            if (bossHeadToPlayers > 0.3f)
                return Status.BT_Failure;
        }

        SetAim();

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // ���� �ֱ⿡ �����ϸ� ���� ����
            bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
            currentTime = bossSO.atkDelay;
        }



        float distanceToTarget = Vector2.Distance(owner.transform.position, target.transform.position);

        if (distanceToTarget > bossSO.attackRange)
        {
            bossAI_Dragon.isAttaking = false;
            return Status.BT_Failure; // ��� ����
        }



        //�ڡڡڼ�����
        //enemyAI.PV.RPC("Filp", RpcTarget.All);;
        //enemyAI.Filp(owner.transform.position.x, target.transform.position.x);

        return Status.BT_Running;
    }
    public void SetAim() // ���ط�, �÷��̾� ��ġ �޾ƿ�
    {
        //�÷��̾ �ٶ󺸵��� ����

        //anim.SetTrigger("Attack"); // ���� �ִϸ��̼�

        //����() ���� �ٲ��� �� -> ����
        Vector3 direction = (target.transform.position - bossAI_Dragon.enemyAim.transform.position).normalized;



        RotateArm(direction);
    }
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        bossAI_Dragon.enemyAim.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    public override void Terminate()
    {
    }

    private void SetStateColor()
    {
        bossAI_Dragon.spriteRenderer.color = Color.black;
    }
}
