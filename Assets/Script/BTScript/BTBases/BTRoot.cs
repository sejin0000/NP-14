using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myBehaviourTree //�������� ��ũ��Ʈ�� �ϳ��� ���� �� �ֳ�?
{

    //��Ʈ ��带 ��Ÿ���� ��ũ��Ʈ[��Ʈ ��� ���� ����&�ʱ�ȭ]

    //��Ʈ ����� ���� : �ٸ� �ൿ Ʈ���� ��带 �ڽ����� ���Խ�Ŵ

    //��Ʈ ����� ������ ������ ������
    //�ٸ� �ൿ Ʈ�� ��带 BTRoot �Ʒ��� �߰��ϸ�, ��ü �ൿ Ʈ���� ������
    //BTRoot�� ����Ͽ� �ൿ Ʈ���� ���۰� ������ �����ϰ�,
    //���� ��带 ���� Ʈ���� ������ ���������� ������.

    //��Tick�޼��带 ���� �ൿ Ʈ���� �����ϰ� ������


    public class BTRoot : BTBehaviour
    {
        //���� ��Ʈ ����� �ڽ� ���
        private BTBehaviour treeChild;

        //�ʱ�ȭ [�ش� ����� Ÿ�� ����&�θ� ��� ����]
        public BTRoot()
        {
            SetNodeType(NodeType.Root);
            SetParent(null);
        }

        //�ڽ� ��� �߰� : �Ķ���͸� ���� ���޵� BTBehaviour�ν��Ͻ���
        //���� ��Ʈ ���(ȣ����)�� �ڽ� ��忡 �Ҵ�
        //���� ���� ��Ʈ ���(ȣ����)�� �θ� ��尡 ��.
        public void AddChild(BTBehaviour newChild)
        {
            treeChild = newChild;
            treeChild.SetParent(this);
        }

        //�ڽ� ��� ��ȯ��
        public BTBehaviour GetChild()
        {
            return treeChild;
        }

        //�θ� ������ �ڽ� ������ �ϴ� ����
        public override void Terminate()
        {
            treeChild.Terminate(); // �ڽ� ��� ���� ����
            base.Terminate(); // ���� ��Ʈ ���(�θ�) ���� ����
        }

        //��Ʈ ��忡�� Ʈ�� ������ ���۵�
        public override Status Tick()
        {
            //�ڽ� Ʈ���� null�� �ƴ� ���
            if (treeChild == null)
            {
                //��� ���� �⺻��
                return Status.BT_Invalid;
            }
            //�ڽ� Ʈ���� ���°��� �⺻�� ���
            else if(treeChild.GetStatus() == Status.BT_Invalid)
            {
                //����ʱ�ȭ&��� ���� �۵������� ����
                treeChild.Initialize();
                treeChild.SetStatus(Status.BT_Running);
            }

            //�ڽ� ����� Update�޼��� ����
            SetStatus(treeChild.Update());

            //����� ���� ��Ʈ ��� ���·� �����ϰ�,
            //�ڽ� ����� ���� ���� ���� ��Ʈ�� �����ϰ� ������
            treeChild.SetStatus(GetStatus());
            

            //���� ��Ʈ ����� ���°� �۵����� �ƴ϶��?
            if(GetStatus() != Status.BT_Running)
            {
                //���� �۾� ����
                Terminate();
            }

            //���� ��Ʈ ����� ���¸� ��ȯ
            return GetStatus();
        }

    }
}
