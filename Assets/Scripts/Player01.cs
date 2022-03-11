using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))] 
public class Player01 : MonoBehaviour
{
    private Rigidbody rb;
    private Gamepad playerAction;
    private InputAction move;

    /*移動速度の設定*/
    [SerializeField] private float playerSpeed = 2.0f;
    /*アニメーター*/
    [SerializeField] Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAction = new Gamepad();
    }

    private void Update()
    {
        ApplyAnimatorParameter();
    }

    private void FixedUpdate()
    {
        var x = move.ReadValue<Vector2>().x;
        
        var moveVector = new Vector3(x, 0, 0);
        var moveDirection = new Vector3(0, 0, x);
        if(moveVector.magnitude > 1)
        {
            moveVector.Normalize();
        }

        var factor = (4 * moveVector.magnitude - rb.velocity.magnitude) / Time.fixedDeltaTime;

        rb.AddForce(moveVector * factor);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(moveDirection), 20.0f * Time.deltaTime);
    }

    private void OnEnable()
    {
        //ボタンが押された瞬間に攻撃する
        playerAction.Player.Attack.started += Attack;

        move = playerAction.Player.Move;
        playerAction.Player.Enable();
    }

    private void OnDisable()
    {
        //この処理がなくなる
        playerAction.Player.Attack.started -= Attack;

        playerAction.Player.Disable();
    }

    private void Attack(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack");
    }

    void ApplyAnimatorParameter()
    {
        var speed = Mathf.Abs(move.ReadValue<Vector2>().x);
        animator.SetFloat("Speed", speed,0.1f,Time.deltaTime);
    }
}