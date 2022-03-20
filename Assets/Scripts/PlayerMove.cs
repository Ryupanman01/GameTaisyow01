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

    /*プレイヤーの方向*/
    private Quaternion player_direction; 


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
        Rotation();

        /*Animatorにキャラクターや環境のパラメーターを設定*/
        ApplyAnimatorParameter();
    }

    private void Move()
    {
        var velocity = rb.velocity;
        var input = Mathf.Abs(move.ReadValue<Vector2>().x);
        var speed = 0f;
        var speeds = move.ReadValue<Vector2>().x * 4.0f;

        /*移動速度を計算*/
        if (input <= 0f)
        {
            state = State.idle;
            speed = 0f;
            Debug.Log("Idle");
        }
        else if (input > 0f && input <= 0.7f)
        {
            state = State.walk;
            speed = move.ReadValue<Vector2>().x * 4.0f;
            Debug.Log("Walk");
        }
        else if (input > 0.7f && input <= 0.9f)
        {
            state = State.run;
            speed = speeds * input * 2.7f;
            Debug.Log("Run");
        }else if(input > 0.9f)
        {
            speed = speeds * input * 3.0f;
            Debug.Log("MaxRun");
        }

        velocity.x = speed;
        rb.velocity = velocity;
    }

    private void Rotation()
    {
        if (move.ReadValue<Vector2>().x > 0)/*左スティックが右に入力されているとき*/
        {
            /*プレイヤーの向きを右に向いている状態にする処理*/
            if (transform.rotation != Quaternion.Euler(0, 0, 0))
            {
                player_direction = Quaternion.Euler(0, 0, 0); /*Quaternion.Eulerで向きを3軸(xyz)まとめて値を指定したものをプレイヤーの向きを入れる変数に代入*/
                transform.rotation = player_direction; /*プレイヤーの向きをlocalRotationに代入して回転させる*/
            }
        }
        else if (move.ReadValue<Vector2>().x < 0)/*左スティックが左に入力されているとき*/
        {
            /*プレイヤーの向きを左に向いている状態にする処理*/
            if (transform.rotation != Quaternion.Euler(0, 180, 0))
            {
                player_direction = Quaternion.Euler(0, 180, 0); /*Quaternion.Eulerで向きを3軸(xyz)まとめて値を指定したものをプレイヤーの向きを入れる変数に代入*/
                transform.rotation = player_direction; /*プレイヤーの向きをlocalRotationに代入して回転させる*/
            }
        }
    }
         

    private void ApplyAnimatorParameter()
    {
        /*Animatorにキャラクターや環境のパラメーターを設定*/
        var speed = Mathf.Abs(move.ReadValue<Vector2>().x);
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
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