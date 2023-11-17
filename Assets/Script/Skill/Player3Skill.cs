using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3Skill : Skill
{
    WeaponSystem _weaponSystem;
    

    public void Start()
    {
        if (photonView.IsMine)
        {
            controller.OnSkillEvent += SkillStart;
        }
        _weaponSystem = GetComponent<WeaponSystem>();

    }
    public override void SkillStart()
    {
        //base.SkillStart();
        if (_weaponSystem.targets.Contains(BulletTarget.Enemy))
        {
            Debug.Log("�� ��� ��ȯ");
            _weaponSystem.targets.Remove(BulletTarget.Enemy);
            _weaponSystem.targets.Add(BulletTarget.Player);
            _weaponSystem.isDamage = false;
        }
        else if (_weaponSystem.targets.Contains( BulletTarget.Player))
        {
            Debug.Log("�� ��� ��ȯ");

            _weaponSystem.targets.Remove(BulletTarget.Player);
            _weaponSystem.targets.Add(BulletTarget.Enemy);
            _weaponSystem.isDamage = true;
        }
        SkillEnd();
    }

    public override void SkillEnd()
    {
        base.SkillEnd();
    }
}
