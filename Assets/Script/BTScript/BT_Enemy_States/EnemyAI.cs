using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

//[Root ���] => �� �׼ǰ� �ٸ��� ��� �ȹ���?
//==>Ư�� AI ���۰� ���¿� �°� �����ϰ� �����ϱ� ���ؼ�
//��� BTRoot ��ü�� �����ϰ�, �� �Ʒ��� ���� ���� �׼� ��带 �߰��ؼ� Ʈ���� ������
//EnemyAI : AI�� ���¿� ������ ������ �����ϰ�, ������
public class EnemyAI : MonoBehaviour
{
    private BTRoot TreeAIState;

    public EnemySO enemySO;          // �÷��̾� ���� [��� Action Node�� owner�� ȹ���Ŵ]
    public BoxCollider2D collider2D; // �ݶ��̴�

    public int currentHP;            // ���� ü�� ���
    public bool isWall = false;      // �� ����

    void Awake()
    {
        //���� ������Ʈ Ȱ��ȭ ��, �ൿ Ʈ�� ����
        CreateTreeATState();
        currentHP = enemySO.hp;
    }
    void Update()
    {
        //AIƮ���� ��� ���¸� �� ������ ���� ����
        TreeAIState.Tick();
    }

    //�ൿ Ʈ�� ���� ����
    void CreateTreeATState()
    {
        //�ʱ�ȭ&��Ʈ ���� ����
        TreeAIState = new BTRoot();

        //BTSelector�� BTSquence ���� : Ʈ�� ���� ����
        BTSelector BTMainSelector = new BTSelector();



        //����(������ : �ϳ��� �����ϸ� ���й�ȯ)
        //��������Ʈ ���� -> �̵�
        BTSquence BTPatrol = new BTSquence();
        //���� �׼��� Squence�� �ڽ����� �߰�

        //EnemyState_Patrol_Move statePatrol_Move = new EnemyState_Patrol_Move(gameObject);
        //BTPatrol.AddChild(statePatrol_Move);
        EnemyState_Patrol_Move state_Patrol_Move = new EnemyState_Patrol_Move(gameObject);
        BTPatrol.AddChild(state_Patrol_Move);



        //����
        //����� üũ(�÷��̾ Ž�� ���� �� ����) -> �÷��̾� �������� �ٶ� -> �÷��̾� ����
        BTSquence BTChase = new BTSquence();
        //���� �׼�&������� Squence�� �ڽ����� �߰�
        EnemyState_Chase_ChaseCondition chaseCondition = new EnemyState_Chase_ChaseCondition(gameObject);
        BTChase.AddChild(chaseCondition);
        EnemyState_Chase_LookAt state_Chase_LookAt = new EnemyState_Chase_LookAt(gameObject);
        BTChase.AddChild (state_Chase_LookAt);
        EnemyState_Chase_Chase stateChase_Chase = new EnemyState_Chase_Chase(gameObject);
        BTChase.AddChild(stateChase_Chase);

        //���� ������ �״�� ���
        //���� -> ����� üũ(�÷��̾ ���� ���� �� ����) -> ���� ���� -> ����� üũ2


        //�ǰ�(��� üũ)
        //���(��� üũ)

        //�����ʹ� �켱���� ���� ������ ��ġ : ���� ���� -> Ư�� ���� -> �÷��̾� üũ(���� ����) -> �̵� ���� ������ ������ ��ġ 
        //���� ������ : Squence�� Selector�� �ڽ����� �߰�(�ڽ� ���� �߿���)
        BTMainSelector.AddChild(BTChase); // ���� ��� O 
        BTMainSelector.AddChild(BTPatrol);             

        //�۾��� ���� Selector�� ��Ʈ ��忡 ���̱�
        TreeAIState.AddChild(BTMainSelector);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            isWall = true;
        }
    }
}
