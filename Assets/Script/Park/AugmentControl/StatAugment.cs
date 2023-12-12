using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAugment
{//공통 요소 이름 코드 설명 등급
    public string Name { get; set; }
    public int Code { get; set; }
    public string func { get; set; }
    public int Rare { get; set; }
}
public class StatAugment : IAugment
{// 스탯은 단순 합연산 이기때문에 스탯값을 모두 가지며 단순 덧셈 함수로 처리하기 위해 모두 구현 
    public string Name { get; set; } = "";
    public int Code { get; set; }
    public string func { get; set; } = "";
    public int Rare { get; set; }
}
public class SpecialAugment : IAugment
{ // 증강별 효과를 코드로 만들것 이기 때문에 필수 요소 4가지만 필요 
    public string Name { get; set; }
    public int Code { get; set; }
    public string func { get; set; }
    public int Rare { get; set; }
    public SpecialAugment(string name, int code, string func, int rare)
    {
        Name = name;
        Code = code;
        this.func = func;
        Rare = rare;
    }
}