using UnityEngine.UI;

using System;
using System.IO;
using System.Text;

using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;

public class CustomRectInt
{
    public RectInt rectInt;


    // ����ȭ
    public static byte[] Serialize(object customobject)
    {
        CustomRectInt cri = (CustomRectInt)customobject;

        // ��Ʈ���� �ʿ��� �޸� ������(Byte)
        MemoryStream ms = new MemoryStream(sizeof(char) + sizeof(int));

        // �� �������� Byte �������� ��ȯ, �������� ���� ������
        ms.Write(BitConverter.GetBytes(cri.rectInt.x), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(cri.rectInt.y), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(cri.rectInt.width), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(cri.rectInt.height), 0, sizeof(int));

        // ������� ��Ʈ���� �迭 �������� ��ȯ
        return ms.ToArray();
    }

    // ������ȭ
    public static object Deserialize(byte[] bytes)
    {
        CustomRectInt ct = new CustomRectInt();
        // ����Ʈ �迭�� �ʿ��� ��ŭ �ڸ���, ���ϴ� �ڷ������� ��ȯ
        ct.rectInt.x = BitConverter.ToInt32(bytes, 0);
        ct.rectInt.y = BitConverter.ToInt32(bytes, sizeof(int));
        ct.rectInt.width = BitConverter.ToInt32(bytes, sizeof(int) * 2);
        ct.rectInt.height = BitConverter.ToInt32(bytes, sizeof(int) * 3);

        return ct;
    }
}

