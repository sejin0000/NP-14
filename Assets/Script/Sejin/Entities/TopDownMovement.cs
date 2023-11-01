using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    TopDownCharacterController _controller;

    private Vector2 _movemewtDirection = Vector2.zero;
    private Rigidbody2D _rigidbody2D;
    private Stats moveSpeed;


    private void Awake()
    {
        _controller = GetComponent<TopDownCharacterController>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _controller.OnMoveEvent += Move;
        moveSpeed = _controller.playerStatHandler.Speed;
    }

    private void FixedUpdate()
    {
        ApplyMovment(_movemewtDirection);
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

    private void Roll()
    {


    }

    private void EndRoll() 
    {
        
    }
}
