using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //キー入力
    public InputAction moveAction;
    public InputAction dashAction;
    public InputAction jumpAction;
    //プレイヤー挙動の変数
    public float speed = 2f;
    public bool isGround = true;
    public float jumpForce = 10f;
    //参照するもの
    [SerializeField] private Animator animator;
    private Transform mainCameraTransform;
    private Rigidbody playerRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction.Enable();
        dashAction.Enable();
        jumpAction.Enable();
        playerRb = GetComponent<Rigidbody>();
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //方向キーの入力受付
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        //ダッシュボタンの受付
        speed = dashAction.IsPressed() ? 4f : 2f;
        //進む方向
        Vector3 moveDirection = Vector3.zero;
        //移動処理
        if (moveInput.magnitude > 0 && mainCameraTransform != null)
        {
            //カメラの正面と右のベクトル
            Vector3 camForward = mainCameraTransform.forward;
            Vector3 camRight = mainCameraTransform.right;
            //Y軸の差文カット
            camForward.y = 0;
            camRight.y = 0;
            //正規化
            camForward.Normalize();
            camRight.Normalize();
            //進行方向の計算
            moveDirection = (camForward * moveInput.y) + (camRight * moveInput.x);
            //正規化
            moveDirection.Normalize();
            //キャラクタの回転方向
            Quaternion targetRotetion = Quaternion.LookRotation(moveDirection);
            //キャラクタの回転
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotetion, Time.deltaTime * 15f);
        }
        //キャラクタの移動
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        //Animatorへの送信
        if (moveInput.magnitude > 0)
        {
            animator.SetFloat("Speed", speed);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
        if (jumpAction.triggered && isGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("Jump", true);
            isGround = false;
            animator.SetBool("isGround", false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("isGround", true);
            animator.SetBool("Jump", false);
        }
    }
}
