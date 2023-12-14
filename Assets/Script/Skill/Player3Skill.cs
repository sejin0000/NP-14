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
            isLink = true;
            controller.SkillMinusEvent += SkillLinkOff;
        }
        _weaponSystem = GetComponent<WeaponSystem>();
        skillIcon = icons[2];
    }

    public override void SkillStart()
    {
        //base.SkillStart();
        if (_weaponSystem.targets.ContainsValue((int)BulletTarget.Enemy))
        {
            Debug.Log("Èú ¸ðµå ÀüÈ¯");
            skillIcon = icons[3];
            _weaponSystem.targets.Remove("Enemy");
            _weaponSystem.targets["Player"] = (int)BulletTarget.Player;
            _weaponSystem.isDamage = false;
        }
        else if (_weaponSystem.targets.ContainsValue((int)BulletTarget.Player))
        {
            Debug.Log("µô ¸ðµå ÀüÈ¯");
            skillIcon = icons[2];
            _weaponSystem.targets.Remove("Player");
            _weaponSystem.targets["Enemy"] = (int)BulletTarget.Enemy;
            _weaponSystem.isDamage = true;
        }
        SkillEnd();
    }

    public override void SkillEnd()
    {
        base.SkillEnd();
    }
}
