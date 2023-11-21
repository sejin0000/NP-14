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


    // 직렬화
    public static byte[] Serialize(object customobject)
    {
        CustomRectInt cri = (CustomRectInt)customobject;

        // 스트림에 필요한 메모리 사이즈(Byte)
        MemoryStream ms = new MemoryStream(sizeof(char) + sizeof(int));

        // 각 변수들을 Byte 형식으로 변환, 마지막은 개별 사이즈
        ms.Write(BitConverter.GetBytes(cri.rectInt.x), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(cri.rectInt.y), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(cri.rectInt.width), 0, sizeof(int));
        ms.Write(BitConverter.GetBytes(cri.rectInt.height), 0, sizeof(int));

        // 만들어진 스트림을 배열 형식으로 반환
        return ms.ToArray();
    }

    // 역직렬화
    public static object Deserialize(byte[] bytes)
    {
        CustomRectInt ct = new CustomRectInt();
        // 바이트 배열을 필요한 만큼 자르고, 원하는 자료형으로 변환
        ct.rectInt.x = BitConverter.ToInt32(bytes, 0);
        ct.rectInt.y = BitConverter.ToInt32(bytes, sizeof(int));
        ct.rectInt.width = BitConverter.ToInt32(bytes, sizeof(int) * 2);
        ct.rectInt.height = BitConverter.ToInt32(bytes, sizeof(int) * 3);

        return ct;
    }
}

