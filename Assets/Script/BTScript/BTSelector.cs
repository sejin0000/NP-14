using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����(Composite) ��� �� �ϳ��� Selector�� �����ϴ� ��ũ��Ʈ
//BTSelector�� �ڽ� ��带 ���������� �����ϸ�
//'ù ��°'�� ������ �ڽ� ��带 ã���� '������ �ڽ� ��带 �������� ����'
namespace myBehaviourTree
{
    //���� ��忡 ���ϹǷ� Composite�� ����Ѵ�.(�ڽ� ��� ���� & ���� ��� �⺻ ����)
    public class BTSelector : BTComposite
{
        //��� ���� ����(��� ��� �� ��)
        public BTSelector()
        {
            SetNodeType(NodeType.Selector);
        }

        //BTSelector�� �ֿ� ���� ���� : ù ���� �ڽ��� ������ �������� �������� ����
        public override Status Update()
        {
            //�ڽ� ��带 ���������� ����
            //���� ������
            for (int i = 0; i < GetChildCount(); i++)
            {
                //�ش� �ڽĳ���� ���� �۵� ���¸� ��ȯ
                Status currentStatus = GetChild(i).Tick();

                //���� ���� ���°� �ƴѰ�� => ���� ������ ���
                //���� ���°� �ƴ� ��쵵 �����ϰ� �� ��???
                //=> ���� ���̰ų� �ߴ� ������ ��, ���� ������ �������̸� �̷��� ������ �� �ִٰ� ����
                //�ൿ Ʈ������ �������̰ų� �ߴܵ� ���¸� ��ǥ�� ���� �� �ְ� ��
                if(currentStatus != Status.BT_Failure)
                {
                    //i(���� �ڽ� ���) ������ ��� �ڽĳ�� �ʱ�ȭ
                    ClearChild(i);
                    //�ش� �ڽ� ����� ����(Status)�� ��ȯ
                    return currentStatus;
                }
            }

            //��� �ڽ� ��� ����or�ڽ� ��� ����
            return Status.BT_Failure;
        }

        //�Ķ���� index�� ������ ��� �ڽĳ�带 �缳��(�ʱ�ȭ, Reset)
        protected void ClearChild(int skipIndex)
        {
            //�ڽ� ��� ����ŭ �ݺ�
            for (int i = 0; i < GetChildCount(); ++i)
            {
                //���� �ڽ��� ���ܽ�Ŵ
                if(i != skipIndex)
                {
                    //�� ��� ����
                    GetChild(i).Reset();
                }
            }
        }
    }
}
