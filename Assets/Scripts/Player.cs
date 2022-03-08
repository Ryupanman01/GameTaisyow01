using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Gamepad playerAction;
    private InputAction move;
    private Rigidbody rb;

    //最高速度
    [SerializeField] private float maxSpeed = 5f;

    //動く方向
    private Vector3 forceDirection = Vector3.zero;


    [SerializeField] private Camera playerCamera;

    private Animator animator;

    //回転の制御
    private Quaternion targetRotation;

    //動けるか動けないかの判定
    private bool canMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAction = new Gamepad();

        canMove = true;
        targetRotation = transform.rotation;
    }

    //表示時
    private void OnEnable()
    {
        //ボタンが押された瞬間に攻撃する
        playerAction.Player.Attack.started += Attack;

        move = playerAction.Player.Move;

        playerAction.Player.Enable();
    }

    //非表示時
    private void OnDisable()
    {
        //この処理がなくなる
        playerAction.Player.Attack.started -= Attack;

        playerAction.Player.Disable();
    }

    private void Update()
    {
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude / maxSpeed * maxSpeed);
    }

    private void FixedUpdate()
    {

        SpeedCheck();

        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera);
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera);

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        LookAt();
    }

    

    private void Attack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack");
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;

        //ベクトルの正規化
        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;

        //ベクトルの正規化
        return forward.normalized;
    }

    private void SpeedCheck()
    {
        Vector3 playerVelocity = rb.velocity;
        playerVelocity.y = 0;

        if(playerVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = playerVelocity.normalized * maxSpeed;
        }
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            targetRotation = Quaternion.LookRotation(direction);
            rb.rotation = Quaternion.RotateTowards(rb.rotation,targetRotation,900 * Time.deltaTime);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
}
