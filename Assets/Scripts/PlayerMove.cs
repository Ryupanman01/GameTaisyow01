using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    /*プレイヤーの移動状態*/
    private enum State
    {
        idle,
        walk,
        run
    }

    private Rigidbody rb;
    private Gamepad inputData;
    private InputAction move;
    private State state;

    [SerializeField] Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        inputData = new Gamepad();
    }

    private void Update()
    {
        /*キャラクターの移動*/
        Move();
        //Rotation();

        /*Animatorにキャラクターや環境のパラメーターを設定*/
        ApplyAnimatorParameter();
    }

    private void Move()
    {
        var velocity = rb.velocity;
        var input = Mathf.Abs(move.ReadValue<Vector2>().x);

        /*移動速度を計算*/
        if (input <= 0f)
        {
            state = State.idle;
            Debug.Log("Idle");
        }
        else if (input > 0f && input <= 0.5f)
        {
            state = State.walk;
            Debug.Log("Walk");
        }
        else if (input > 0.5f && input <= 1f)
        {
            state = State.run;
            Debug.Log("Run");
        }
        var speed = move.ReadValue<Vector2>().x * 4.0f;

        velocity.x = speed;
        rb.velocity = velocity;
    }

    private void ApplyAnimatorParameter()
    {
        /*Animatorにキャラクターや環境のパラメーターを設定*/
        var speed = Mathf.Abs(move.ReadValue<Vector2>().x);
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    /*加速*/
    private void Acceleration()
    {

    }

    private void OnEnable()
    {
        move = inputData.Player.Move;
        inputData.Player.Enable();
    }

    private void OnDisable()
    {
        inputData.Player.Disable();   
    }
}