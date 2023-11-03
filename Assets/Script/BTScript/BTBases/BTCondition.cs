using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--
//EBH_Running �������� ���� : EBH_Success or EBH_Failure
//Initialize(), Terminate() ��� ���� : ��? �ǹ� ���� �Լ���(���Ǹ� üũ��)
//--

//������(Sequence)��忡�� ���� ���� ���ϴ� ���
//�ൿ Ʈ������ ���Ǵ� ����(Condition) ��带 �����ϴ� ��ũ��Ʈ
//���� ���� �ַ� �� ���� ����, ���� Ȥ�� ���и� ������
//Ư�� ������ �Ǵ��ϰ� �� ����� ���� ���� ���� �����Ѵ�.
namespace myBehaviourTree
{
    public class BTCondition : BTBehaviour
    {
        //��� Ÿ�� ����(�� ��忡 �� �����ؾ���)
        public BTCondition()
        {
            SetNodeType(NodeType.Condition);
        }

        //���� ����� �ֿ� ������ ������
        public override Status Tick()
        {
            //��� ���� ������Ʈ
            SetStatus(Update());

            if (GetStatus() == Status.BT_Running)
            {
                Debug.Log("EBH_Running�� ������� �ʽ��ϴ�. EBH_Success Ȥ�� EBH_Failure�� ��ȿ�� �����Դϴ�.");
            }

            //������ȭ �ʿ��� : ���� Action node�� �����ϴ� �ʵ� ������ ����� ���

            //���� : EBH_Success�� ��� ==> �۵����� Action ����[�ٸ� ������ �浹 ����/�ൿ Ʈ������ �ϰ��� ����]
            if (GetStatus() == Status.BT_Success)
            {
                //������ ������ �ٸ� ��� �� �ʱ�ȭ - �ݵ�� ���� ���ǹ� �ʿ���
                EndRunningByAction();
            }

            return GetStatus();
        }

        //���� ���� ���� ���õ� �۵� ���� �׼��� �����ϴ� �޼���
        //��Ʈ ��� Ž�� -> �۵� ���� �׼� Ž�� ->
        //Ž���� ��尡 ���� ���� ���� ���� �θ� ������ ���� �ʰų�, �θ� Sequence�� �ƴ� ���
        //==> �۵� ���� �׼� ����
        public void EndRunningByAction()
        {
            //��Ʈ ���� ����
            BTBehaviour BTFindRoot = null;

            //���� ī��Ʈ : �����ϴ� ����? => �ݺ� �۾� ����
            //�ൿ Ʈ�� ������ �߸��� ��쳪 ���� ������ ���� �� ���α׷��� ���°� ����
            int ErrorCount = 0;

            //���� ���� ��尡 ���� �ൿ Ʈ���� ��Ʈ ��带 ã�� ���� ����
            //�ൿ Ʈ�� : ���� ���� // ��Ʈ ��� : Ʈ���� �ֻ��� ��� => ��� ���� ��Ʈ ��忡�� �Ļ���


            //1. ���� ���� ����� �θ� ��� �Ҵ�
            BTFindRoot = GetParent();

            if (BTFindRoot != null)
            {
                //2. �ִ� 100�� �ݺ� : �� 100��? : Ʈ���� ���⵵�� ���� ������ �޶�����(�������� �ֻ��� ������ Ž���ؾ� �ϹǷ�)
                while (ErrorCount < 100)
                {
                    //3. �� �ݺ����� BTFindRoot�� ���� BTFindRoot�� �θ� ��带 �Ҵ�.
                    //==>���� �Ҹ���? : ���� ������ ���� �ݺ��ϸ� �θ� ��带 ã�Ƽ� ���� �ö�
                    BTFindRoot = BTFindRoot.GetParent();

                    //�׷��� �ö󰡴ٰ� �θ� ��尡 ���� ��찡 �Ǹ�?(�ֻ��� ��� : ��Ʈ ��忡 ����)
                    if (BTFindRoot.GetParent() == null)
                    {
                        //�ݺ� Ż��
                        break;
                    }

                    ++ErrorCount;
                }
            }

            //������ ã�� ��Ʈ ����� ���¸� Ȯ�� ��, �׼� ��带 ã�� ó��
            //���� �Ʒ� if�� ���� �߿�


            //��Ʈ ��带 ã�Ҵ��� üũ(ã�� ��� �Ʒ��� ����)
            if (BTFindRoot != null)
            {
                //��Ʈ ��� ���� üũ(�ൿ Ʈ���� �۵� ������ Ȯ��)
                if (BTFindRoot.GetStatus() == Status.BT_Running)
                {
                    //���� �������� �׼� ��带 ã�Ƽ� BTRunningAction�� �Ҵ�
                    //�̶�, �Ķ���� �� ((BTRoot)BTFindRoot).GetChild()�� ���� 
                    //��Ʈ ����� ù �ڽ� ��� ���� Ž���ϵ��� ��
                    BTBehaviour BTRunningAction = FindRunningAction(((BTRoot)BTFindRoot).GetChild());

                    //�������� '�׼�' ��带 ã�� ���
                    if (BTRunningAction != null)
                    {
                        //���� this Condition�� Running Action�� �ٸ� �θ� �����ų�
                        //���� ����� �θ� Sequence�� �ƴ϶�� Terminate ȣ��
                        if (GetParent() != BTRunningAction.GetParent() || GetParent().GetNodeType() != NodeType.Sequence)
                        {
                            //�������� �׼��� ����
                            BTRunningAction.Terminate();
                        }

                    }
                }
            }
        }


        //���� ���� �׼� ��带 Ž��[���������]
        //�Ķ���Ͱ����� ���� ���� ���� �׼� ��带 Ž���Ѵ�.
        public BTBehaviour FindRunningAction(BTBehaviour BTChild)
        {
            //���� ���� �׼� ��� �����
            BTBehaviour BTRunningAction = null;

            //�Ķ���� ���� null�� �ƴ� ���(��� ����)
            if (BTChild != null)
            {
                //BTChild�� ��Ʈ Ÿ���� Selector �ų�, Sequence�� ��츸 ����(����(Composite) ����� ���)
                if (BTChild.GetNodeType() == NodeType.Selector || BTChild.GetNodeType() == NodeType.Sequence)
                {
                    //���� ����� ��� �ڽ� ��带 üũ(���� ��� ��ũ��Ʈ�� GetChildCount ���)
                    for (int i = 0; i < ((BTComposite)BTChild).GetChildCount(); i++)
                    {
                        //��������� FindRunningAction �޼��带 ȣ���Ͽ�, ���� �ڽ� ��忡 ����
                        //���� ���� �׼� ��带 Ž���ϰ�, �װ��� BTRunningAction�� ����
                        //�̰��� ���� ���� ��� �Ʒ��� ��� ���� ��忡 ���� ���� ���� �׼� ��带 ã�ƾ��ϹǷ�
                        //����� ȣ���� ����ȴ�.
                        BTRunningAction = FindRunningAction(((BTComposite)BTChild).GetChild(i));

                        //���� ���� �׼� ��带 ã�����, �ش� ��带 ��ȯ�ϰ� Ž�� ����
                        if (BTRunningAction != null)
                            return BTRunningAction;
                    }
                }

                //���� ���� �׼� ������� �Ǻ� : ������� Action & ���� ������
                if (BTChild.GetNodeType() == NodeType.Action && BTChild.GetStatus() == Status.BT_Running)
                {
                    //�������� �׼� ��� ��ȯ => ���ȣ�� �ߴ� => ���� ���� �׼� ��尡 ��ȯ
                    return BTChild;
                }
            }

            //��� �ڽ� ��忡�� ���� ���� �׼��� ã�� ���� ��� ==> null ��ȯ
            return BTRunningAction;
        }

        //��� ���� BTChild��� ��ü�� �Ķ���ͷ� ���� null�̶�� �ش� ���� Ʈ������ ���� ���� �׼� ��尡 �����Ƿ� null ��ȯ ������
        //�� ���� ��� BTChild�� ������ üũ, ���� ����� �ڽ� ��忡 ���ؼ� ��������� FindRunningAction�� ȣ����
        //=> �ڽ� ��� �߿��� ���� ��尡 �ƴ� ���, �׼� ��带 �Ǻ� ������
    }

}