using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearPanel : UIBase
{

    public override void Initialize()
    {
        // Ư�� state���� ����Ǵ� �̺�Ʈ�� �߰��ϰ� ������ ��..
        MainGameManager.Instance.OnEndStateChanged += Open;
    }
    
}
