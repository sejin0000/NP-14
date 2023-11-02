using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sample_Player
{
    public int Atk;
    public int HP;
    public float AtkSpeed;
    public int MoveSpeed;
    public int CoolDown;
    public int CritRate;
    public int NumberOfShots;
    public int ShotSpread;
    public int RollCoolDown;

    public Sample_Player(int type)
    {
        switch (type)
        {
            case 0:
                Atk = 10;
                HP = 100;
                AtkSpeed = 1.2f;
                MoveSpeed = 7;
                CoolDown = 15;
                CritRate = 40;
                NumberOfShots = 30;
                ShotSpread = 15;
                RollCoolDown = 4;
                break;
            case 1:
                Atk = 10;
                HP = 150;
                AtkSpeed = 0.7f;
                MoveSpeed = 7;
                CoolDown = 10;
                CritRate = 30;
                NumberOfShots = 2;
                ShotSpread = 10;
                RollCoolDown = 6;
                break;
            case 2:
                Atk = 10;
                HP = 90;
                AtkSpeed = 0.6f;
                MoveSpeed = 7;
                CoolDown = 0;
                CritRate = 50;
                NumberOfShots = 5;
                ShotSpread = 30;
                RollCoolDown = 10;
                break;
        }
    }
}


