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
    
    void Awake()
    {
        //���� ������Ʈ Ȱ��ȭ ��, �ൿ Ʈ�� ����
        CreateTreeATState();
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
        EnemyState_Patrol_WayPoint state_Patrol_WayPoint = new EnemyState_Patrol_WayPoint(gameObject);
        BTPatrol.AddChild(state_Patrol_WayPoint);
        EnemyState_Patrol_Rotation statePatrol_Rotation = new EnemyState_Patrol_Rotation(gameObject);
        BTPatrol.AddChild(statePatrol_Rotation);



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


        //���� ������ : Squence�� Selector�� �ڽ����� �߰�(�ڽ� ���� �߿���)
        BTMainSelector.AddChild(BTPatrol);
        BTMainSelector.AddChild(BTChase); // ���� ��� O      

        //�۾��� ���� Selector�� ��Ʈ ��忡 ���̱�
        TreeAIState.AddChild(BTMainSelector);
    }
}
