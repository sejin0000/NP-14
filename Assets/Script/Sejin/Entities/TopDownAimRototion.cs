using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownAimRototion : MonoBehaviour
{
    [SerializeField] private Transform armPivot;

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
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3((-1 * transform.localScale.x), transform.localScale.y, transform.localScale.z);
                armPivot.localScale = new Vector3((-1 * armPivot.localScale.x), (-1 * armPivot.localScale.y), armPivot.localScale.z);
            }
        }
        else
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3((-1 * transform.localScale.x), transform.localScale.y, transform.localScale.z);
                armPivot.localScale = new Vector3((-1 * armPivot.localScale.x), (-1 * armPivot.localScale.y), armPivot.localScale.z);
            }
        }

        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
