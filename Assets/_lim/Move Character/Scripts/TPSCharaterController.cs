using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TPSCharaterController : MonoBehaviour
{
    [SerializeField] private Transform cameraArm;
    [SerializeField] private Collider AttackRange;

    private Collider attackCollider;
    private Rigidbody rigid;
    public Animator animator;
    private float movespeed = 0f;
    private bool JDown;
    private bool isJump = false;
    private bool isDeath = false;
    private bool isHit = false;

    public TrailRenderer trailEffect;
    public bool punchReady = true;
    public bool axeReady = false;
    public bool pickaxeReady = false;

    [SerializeField] private GameObject[] gameObjects;
    public Dictionary<string, GameObject> ObjDict = new Dictionary<string, GameObject>();


    void Start()
    {
        ObjDict.Add("axe", gameObjects[0]);
        ObjDict.Add("pickaxe", gameObjects[1]);
        
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        attackCollider = AttackRange.GetComponent<Collider>();
        attackCollider.enabled = false;
    }

    public void SetItem(string objectName, bool isActive)
    {
        GameObject obj;
        if(ObjDict.TryGetValue(objectName, out obj))
        {
            obj.SetActive(isActive);
        }
    }
    


    
    void Update()
    {
        if (!isDeath && !GetComponent<Gathering>().ingGathering)
        {
            Move();
            jump();
            OnDeath();
            Attack();
        }
        LookAround();
        
    }
    


    private void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        animator.SetFloat("speed", movespeed);
        if (isMove)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movespeed = 6;
            }
            else
            {
                movespeed = 3;
            }

            var forward = cameraArm.forward;
            Vector3 lookForward = new Vector3(forward.x, 0f, forward.z).normalized;
            var right = cameraArm.right;
            Vector3 lookRight = new Vector3(right.x, 0f, right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            var transform1 = transform;
            transform1.forward = moveDir;
            transform1.position += moveDir * (Time.deltaTime * movespeed);
            
        }
        else movespeed = 0;

        // Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized , Color.red);
    }
    
    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) ;
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;
            
        if (x < 180)
        {
            x = Mathf.Clamp(x, -1f, 55f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        cameraArm.position = transform.position;
    }

    private void jump()
    {
        JDown = Input.GetButtonDown("Jump");
        float jumpPower = 3f;
        
        if (JDown && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animator.SetTrigger("jump");
            //isJump = true;
        }
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isHit && punchReady)
        {
            isHit = true;
            animator.SetTrigger("attack");
            StartCoroutine(AttackDelay());
        }
        
        if (Input.GetMouseButtonDown(0) && !isHit && axeReady)
        {
            isHit = true;
            animator.SetTrigger("toolAT");
            StartCoroutine(AttackDelay());
        }
        
        if (Input.GetMouseButtonDown(0) && !isHit && pickaxeReady)
        {
            isHit = true;
            animator.SetTrigger("toolAT");
            StartCoroutine(AttackDelay());
        }
    }
    
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.6f);
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.4f);
        attackCollider.enabled = false;
        isHit = false;
    }
    
    private void OnDeath()
    {
        if (HPBar.curHp <= 0)
        {
            isDeath = true;
            animator.SetTrigger("death");
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("floor")) return;
        isJump = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("floor")) return;
        isJump = true;
        
        
    }

    
    
    
}
