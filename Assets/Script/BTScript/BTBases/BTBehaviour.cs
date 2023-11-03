using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BT�� �⺻���� ���� ��ũ��Ʈ
//��ü���� �ൿ Ʈ�� ���� �� Ŭ������ ����Ͽ� ����
//'Tick()'�޼��带 ȣ���Ͽ� ��带 �����ϰ�, ���¸� ������Ʈ�ϴ� ����
//�ൿ Ʈ���� ������ �ֵ��ϴ� �ٽ��̴�.


//���� �����̽� ���� => �Ʒ� ����(enumŸ���� ������ class����)
//��� �� ���ϴ� Ŭ�������� using ���ӽ����̽��̸� �� ���ؼ� ��밡��
//�ش� ���ӽ����̽� ����� virtual �޼��忡�� ovveride(������) ���� ����
namespace myBehaviourTree
{
    //����� ���� ��� ����(�� ����� ���� ����� ��ȯ��)
    public enum Status
    {
        BT_Invalid, // �湮 ��尡 �ƴ� �� �⺻(�ּ�) ���°�
        BT_Success, // ���������� �Ϸ��
        BT_Failure, // �Ϸ����� ����
        BT_Running, // ���� �������� Action��
        BT_Aborted, // �ߴܵ�
    };

    //�ൿ Ʈ���� �پ��� ��� ���� ����
    //�� ���� �� ���� �� �ϳ��� ������ ����
    public enum NodeType
    {
        Root,
        Selector,
        Sequence,
        Paraller,
        Decorator,
        Condition,
        Action,
    };

    public class BTBehaviour
    {
        //�Ʒ� �ΰ��� �߿���
        private Status status; // �ʱ� �⺻�� EBH_Invalid
        private NodeType nodeType; // ��� Ÿ��
        private int index; // ��� �ε���
        private BTBehaviour treeParent; // �θ� ���

        public BTBehaviour()
        {
            status = Status.BT_Invalid;
        }

        //����� ���� ���� ��ȯ �޼���(�Ϸ� Ȥ�� ���� �� true)
        public bool IsTerminated()
        {
            //��Ʈ or ������ | : �� �� �ϳ��� ���̶�� true�� ��ȯ�Ѵ�.
            return status == Status.BT_Success | status == Status.BT_Failure;
        }

        //����� �۵��� ���� ��ȯ �޼���
        public bool IsRunning()
        {
            return status == Status.BT_Running;
        }



        //�θ� ��� ����&��ȯ
        public void SetParent(BTBehaviour NewParrent)
        {
            treeParent = NewParrent;
        }
        public BTBehaviour GetParent()
        {
            return treeParent;
        }

        //���� ����&��ȯ
        public void SetStatus(Status NewStatus)
        {
            status = NewStatus;
        }
        public Status GetStatus()
        {
            return status;
        }

        //���� ��� ����&��ȯ
        public void SetNodeType(NodeType NewNodeType)
        {
            nodeType = NewNodeType;
        }
        public NodeType GetNodeType()
        {
            return nodeType;
        }

        //�ε��� ����&��ȯ
        public void SetIndex(int NewIndex)
        {
            index = NewIndex;
        }
        public int GetIndex()
        {
            return index;
        }


        //��� ���� �ʱ�ȭ
        public virtual void Reset()
        {
            status = Status.BT_Invalid;
        }
        

        //���� �޼��� = ��带 �ʱ�ȭ�ϴ� ��� ȣ����(�ٸ� ������ ������)
        public virtual void Initialize()
        {
            
        }

        //���� �޼��� = ��带 �����ϴ� ��� ȣ����(�ٸ� ������ ������)
        public virtual void Terminate()
        {

        }


        //���� ������Ʈ�� �����ϰ�, �ش� ���¸� ��ȯ(�ٸ� Ŭ�������� ������ ����)
        public virtual Status Update()
        {
            return Status.BT_Success;
        }

        //��带 �����ϰ�, ���¸� ������Ʈ�ϰ�, �ʿ��� ��� �ʱ�ȭ �� ���Ḧ ����
        //���� Update()�� �Բ� �����
        public virtual Status Tick()
        {
            //���� ���°� �⺻���̶��
            if(status == Status.BT_Invalid)
            {
                //�ʱ�ȭ ����
                Initialize();
                //��� ���� �۵������� ����
                status = Status.BT_Running;
            }

            //Ư���� ��찡 �ƴ϶�� ���� ������Ʈ �޼��带 ���� ���� ���¸� ����
            //status�� EnumŸ���̸�, Update�� �� Ÿ���� �ϳ��� ��ȯ�ϹǷ� ���԰���
            status = Update();


            //���� ���°� �۵����� �ƴ϶��
            if(status != Status.BT_Running)
            {
                //���� ����
                Terminate();
            }

            //��������� ���� ���¸� ��ȯ
            return status;
        }
    }
}
