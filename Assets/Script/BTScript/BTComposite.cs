using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ൿ Ʈ���� ����(Composite)��带 �����ϴ� ��ũ��Ʈ

namespace myBehaviourTree
{
    public class BTComposite : BTBehaviour
    {
        //BTComposite ��ü�� ������ �ڽ� ������ ����Ʈ
        //==>���� ��忡 ���� �ڽ� ��带 ������
        protected List<BTBehaviour> listChild;

        //�ڽ� ����Ʈ �ʱ�ȭ
        public BTComposite()
        {
            listChild = new List<BTBehaviour>();
        }

        //Ư�� �ε����� �ڽ� ��带 ��ȯ��
        public BTBehaviour GetChild(int index)
        {
            return listChild[index];
        }

        //���� ���� ��忡 ���� �ڽĳ���� ������ ��ȯ��
        public int GetChildCount()
        {
            return listChild.Count;
        }

        //��� �ڽ� ������ Reset�Ͽ� ��带 �ʱ� ���·� ����
        public override void Reset()
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                GetChild(i).Reset();
            }
        }

        //�ڻ��ο� �ڽ� ��� �߰�
        //List�� ����&���� ����
        //=>�� �ڽ� ��忡 �θ� ��� ����(���� BTComposite)
        public void AddChild(BTBehaviour newChild)
        {
            listChild.Add(newChild);
            //�߰��� Child�� Index����(�����ϱ� �߿�)
            newChild.SetIndex(listChild.Count - 1);
            //�߰��� Chilid�� �θ� ����
            newChild.SetParent(this);
        }
    }
}
