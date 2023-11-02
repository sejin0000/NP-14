using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����(Composite) ��� �� �ϳ��� Squence�� �����ϴ� ��ũ��Ʈ
//BTSquence�� �ڽ� ��带 ���������� �����ϸ�
//�ڽ� ��� �� �ϳ��� �����ϸ� �����ϰ�, ��� �ڽ� ��尡 �����ؾ� �����ϴ� �����
namespace myBehaviourTree
{
    public class BTSquence : BTComposite
    {
        //��� ���� ����(��� ��� �� ��)
        public BTSquence()
        {
            SetNodeType(NodeType.Sequence);
        }

        //BTSquence ����� �ֿ� ���� ���� : �ڽ� ��� �� �ϳ��� �����ϸ� �����ϰ�, ��� �ڽ� ��尡 �����ؾ� ����
        public override Status Update()
        {
            //���� BTSquence ���� ����(�켱 �ʱ�ȭ)
            Status currenStatus = Status.BT_Invalid;

            //�ڽ� ��� ��ȸ
            for (int i = 0; i < GetChildCount(); i++)
            {
                //�� �ݺ��� �ڽ� ��� ���¸� ������ ����
                currenStatus = GetChild(i).GetStatus();

                //�ڽ� ��� Ÿ���� �׼��� �ƴϰų�, ��� Status�� ���� ���°� �ƴ� ���
                //�̹� ������ �ڽ� ���� �ٽ� ������Ʈ ���ص� �������Ƿ� �̿Ͱ��� �����Ѵ�.
                //�ھ׼� ��带 �����ϴ� ������ �׼� ���� �ѹ� ����Ǹ� ���� Ȥ�� ���з� �����ǹǷ�
                //������ ���� ������Ʈ�� �ʿ���� �����̴�.
                if (GetChild(i).GetNodeType() != NodeType.Action || GetChild(i).GetStatus() != Status.BT_Success)
                {
                    //�ڽ� ��� ������Ʈ                    
                    currenStatus = GetChild(i).Tick();
                }                   

                //������Ʈ �� ���� Ȯ��

                //���� ���°� �ƴ϶��
                if (currenStatus != Status.BT_Success)
                {
                    //���� ��ȯ(�ϳ��� �����ϸ� BTSquence�� ���и� ��ȯ��)
                    return currenStatus;
                }                   
            }

            //���� ���ǿ� �ɷ� ��ȯ���� �ʾҴٸ� ��� ���������Ƿ�
            //BTSquence ���� ������ ��ȯ�ϰ� ��
            return Status.BT_Success;
        }
    }
}

