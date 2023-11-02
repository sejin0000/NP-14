using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    TopDownCharacterController _controller;

    private Vector2 _movemewtDirection = Vector2.zero;
    private Rigidbody2D _rigidbody2D;
    private Stats moveSpeed;
    private Vector2 mousePos;

    public bool isRoll = false;

    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
        _controller.OnRollEvent += Roll;
        _controller.OnLookEvent += MousePos;
        moveSpeed = _controller.playerStatHandler.Speed;
    }

    private void FixedUpdate()
    {
        if (!isRoll)
        {
            ApplyMovment(_movemewtDirection);
        }
        else
        {
            ApplyRolling(mousePos);
        }
    }

    private void Move(Vector2 direction)
    {
        _movemewtDirection = direction;
    }

    private void ApplyMovment(Vector2 direction)
    {
        direction = direction * moveSpeed.total;
        _rigidbody2D.velocity = direction;
    }
    private void ApplyRolling(Vector2 direction)
    {
        direction = direction * moveSpeed.total * 2f;
        _rigidbody2D.velocity = direction;
    }

    private void Roll()
    {
        isRoll = true;
        Invoke("EndRoll",0.6f);
    }

    private void EndRoll() 
    {
        isRoll = false;
    }

    private void MousePos(Vector2 _mousePos)
    {
        if (!isRoll)
        {

            mousePos = _mousePos.normalized;
        }
    }
}
