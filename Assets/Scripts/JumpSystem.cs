using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpSystem : MonoBehaviour
{
    private CharacterController characterController;
    private Rigidbody rb;
    private Gamepad playerAction;
    private InputAction move;
    [SerializeField] Animator animator;

    //速度
    private Vector3 velocity;
    //移動スピード
    [SerializeField] private float speed = 2f;
    //ジャンプ力
    [SerializeField] private float jumpPower = 6f;
    //現在のジャンプ値
    private float currentJumpValue;
    //2段階目のジャンプ
    [SerializeField] private float doubleJumpPower = 6f;
    //最初のジャンプをしているかどうか
    [SerializeField] private bool isFirstJump;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        playerAction = new Gamepad();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
       
    }

    private void Jumps()
    {
        //接地時の処理
        if (characterController.isGrounded)
        {
            velocity = Vector3.down;
            animator.SetBool("IsGrounded", true);
            //ジャンプ
        }
    }

    private void OnEnable()
    {
        playerAction.Player.Jump.started += Jump;

        move = playerAction.Player.Move;
        playerAction.Player.Enable();
    }

    private void OnDisable()
    {
        playerAction.Player.Jump.started -= Jump;

        playerAction.Player.Disable();
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Jump");
    }
}
