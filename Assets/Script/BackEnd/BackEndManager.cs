using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BackEndManager : MonoBehaviour
{
    void Start()
    {
        var bro = Backend.Initialize(true); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻� 
        }

        Test();
    }

    async void Test()
    {
        await Task.Run(() => {
            //BackendLogin.Instance.CustomSignUp("user2", "1234"); // [�߰�] �ڳ� ȸ������ �Լ�
            Debug.Log("�׽�Ʈ�� �����մϴ�.");
        });
    }
}
