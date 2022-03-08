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

    private float move_x;
    private Vector3 player_move;

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
        //rb.AddForce(, ForceMode.Impulse);
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