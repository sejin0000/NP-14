using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameCamera : MonoBehaviour
{
    public GameObject Target;               // ī�޶� ����ٴ� Ÿ��

    public float offsetX = 0.0f;            // ī�޶��� x��ǥ
    public float offsetY = 0.0f;            // ī�޶��� y��ǥ
    public float offsetZ = -10.0f;          // ī�޶��� z��ǥ

    public float CameraSpeed = 10.0f;       // ī�޶��� �ӵ�
    Vector3 TargetPos;                      // Ÿ���� ��ġ

    private void Start()
    {
        if (TestGameManager.Instance != null)
        {
            Target = TestGameManager.Instance.InstantiatedPlayer;
        }
        if (TestGameManagerSejin.Instance != null)         
        { 
            Target = TestGameManagerSejin.Instance.InstantiatedPlayer;
        }
        //Target = TestGameManagerDohyun.Instance.InstantiatedPlayer;
    }

    void FixedUpdate()
    {
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }
}
