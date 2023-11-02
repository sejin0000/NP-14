using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//행동 트리의 복합(Composite)노드를 구현하는 스크립트

namespace myBehaviourTree
{
    public class BTComposite : BTBehaviour
    {
        //BTComposite 객체가 가지는 자식 노드들의 리스트
        //==>복합 노드에 속한 자식 노드를 저장함
        protected List<BTBehaviour> listChild;

        //자식 리스트 초기화
        public BTComposite()
        {
            listChild = new List<BTBehaviour>();
        }

        //특정 인덱스의 자식 노드를 반환함
        public BTBehaviour GetChild(int index)
        {
            return listChild[index];
        }

        //현재 복합 노드에 속한 자식노드의 개수를 반환함
        public int GetChildCount()
        {
            return listChild.Count;
        }

        //모든 자식 노드들을 Reset하여 노드를 초기 상태로 돌림
        public override void Reset()
        {
            for (int i = 0; i < GetChildCount(); i++)
            {
                GetChild(i).Reset();
            }
        }

        //★새로운 자식 노드 추가
        //List에 저장&순서 설정
        //=>각 자식 노드에 부모 노드 설정(현재 BTComposite)
        public void AddChild(BTBehaviour newChild)
        {
            listChild.Add(newChild);
            //추가된 Child의 Index설정(순서니까 중요)
            newChild.SetIndex(listChild.Count - 1);
            //추가된 Chilid의 부모 설정
            newChild.SetParent(this);
        }
    }
}
