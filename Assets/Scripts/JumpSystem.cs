using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpSystem : MonoBehaviour
{
    private Rigidbody rb;
    private bool isGrounded;
    public float jumpPower;

    private Ray ray;                /*飛ばすレイ*/
    private float distance = 0.5f;  /*レイを飛ばす距離*/
    private RaycastHit hit;         /*例が何かに当たった時の情報*/
    private Vector3 rayPosition;    /*レイを発射する位置*/

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    
}
