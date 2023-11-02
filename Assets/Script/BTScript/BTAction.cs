using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BT�� ���� ���� ��� �� �ϳ� => ���� ���� ������ ����� �׼��� ����
namespace myBehaviourTree
{
    public class BTAction : BTBehaviour
    {
        //--
        //Left Node
        //Actor�� ���¸� ó���ϴ� Ŭ����
        //���ش� Action Node�� EBH_Success ���°�, ��湮 �� skip
        //--

        public BTAction()
        {
            SetNodeType(NodeType.Action);
        }

        //����Ʈ ���� : ����� => �ʿ��� ��쿡 ovrerride�ؼ� ���

        //�ൿ ��� ���� �� �۵�(�߰����� �۾��� �ʿ��� ��� �߰�)
        public override void Initialize()
        {

        }
        //�ൿ ��� ���� �� �۵�(�߰����� �۾��� �ʿ��� ��� �߰�)
        public override void Terminate()
        {

        }

        //�ൿ ����� ���¸� �ʱ�ȭ
        //�ൿ ��� ���� �� ���� ���¸� ������ �� ȣ��
        //����� ���� �ʱ�ȭ�� �س���
        public override void Reset()
        {
            SetStatus(Status.BT_Invalid);
        }

        //�ൿ ��带 �����ϰ� ���¸� ��ȯ��
        public override Status Tick()
        {
            //���°� �⺻�� ���
            if (GetStatus() == Status.BT_Invalid)
            {
                //�ʱ�ȭ �۾� ����
                Initialize();
                SetStatus(Status.BT_Running);
            }

            //���� �ൿ�� ����, ���� ����
            SetStatus(Update());

            //�۵����� �ƴ϶��
            if (GetStatus() != Status.BT_Running)
            {
                //�ʱ�ȭ
                Terminate();
            }

            //���������� ���� ��� ���� ��ȯ
            return GetStatus();
        }
    }

}