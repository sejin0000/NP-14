using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3Skill : Skill
{
    WeaponSystem _weaponSystem;
    

    public override void Start()
    {
        base.Start();
        _weaponSystem = GetComponent<WeaponSystem>();

    }
    public override void SkillStart()
    {
        base.SkillStart();
        if (_weaponSystem.target == BulletTarget.Enemy)
        {
            Debug.Log("�� ��� ��ȯ");
            _weaponSystem.target = BulletTarget.Player;
            _weaponSystem.isDamage = false;
        }
        else if (_weaponSystem.target == BulletTarget.Player)
        {
            Debug.Log("�� ��� ��ȯ");

            _weaponSystem.target = BulletTarget.Enemy;
            _weaponSystem.isDamage = true;
        }
        SkillEnd();
    }

    public override void SkillEnd()
    {
        base.SkillEnd();
    }
}
