using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultController : MonoBehaviour
{
    public AugmentManager augmentManager;
    public ResultManager resultManager;

    public void MakeManager() 
    {
        ResultManager Prefab = Resources.Load<ResultManager>("Prefabs/ResultManager");
        //������ �ε��ϴ°ɷ� ���ҽ� �ε� = �ҷ����Ⱑ ��û���ſ� =���۶� ó���Ұ�, Ȥ�� �ּ��Ѹ� �÷����ϰ� ��ġ�� �صѰ�
        //�������� �ѹ��� �ε��Ѵ� ��ŸƮ Ȥ�� �����ũ���� �ε��ؼ� �����Ұ�
        resultManager = Instantiate(Prefab);
        augmentManager = new AugmentManager();
    }
}
