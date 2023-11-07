using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRototion : MonoBehaviour
{
    
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private GameObject weaponPivot;


    private TopDownCharacterController _controller;
    private PlayerAnimatorController _animator;

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
    }
    private void Start()
    {
        _controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 aimDirection)
    {
        RotateArm(aimDirection);
    }

    private void RotateArm(Vector2 aimDirection)
    {
        float rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg ;

        if (Mathf.Abs(rotZ) > 90f)
        {
            if (playerSprite.transform.localScale.x > 0)
            {
                playerSprite.transform.localScale = new Vector2((playerSprite.transform.localScale.x * -1), playerSprite.transform.localScale.y);
                weaponPivot.transform.localPosition = new Vector2(weaponPivot.transform.localPosition.x * -1, weaponPivot.transform.localPosition.y);
                weaponPivot.transform.localScale = new Vector2(weaponPivot.transform.localScale.x, weaponPivot.transform.localScale.y * -1);
            }
        }
        else
        {
            if (playerSprite.transform.localScale.x < 0)
            {
                playerSprite.transform.localScale = new Vector2((playerSprite.transform.localScale.x * -1), playerSprite.transform.localScale.y);
                weaponPivot.transform.localPosition = new Vector2(weaponPivot.transform.localPosition.x * -1, weaponPivot.transform.localPosition.y);
                weaponPivot.transform.localScale = new Vector2(weaponPivot.transform.localScale.x, weaponPivot.transform.localScale.y * -1);

            }
        }

        weaponPivot.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
