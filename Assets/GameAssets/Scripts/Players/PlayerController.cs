using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAssets.Scripts.Weapons;
using System;



namespace GameAssets.Scripts.Players
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        CharacterController cc;
        Animator animator;
        [SerializeField]
        protected float moveSpeed = 5f;
        [SerializeField]
        protected float gravity = -9.81f;
        [SerializeField]
        protected float mouseSpeed = 100f;
        [SerializeField]
        protected float minX = -60f;
        [SerializeField]
        protected float maxX = 60f;
        float speed;
        [SerializeField]
        Transform player;
        [SerializeField]
        Transform cameraPivot;

        public Weapon weapon;
        float xRotation = 0f;
        float yaw = 0f;
        float verticalVelocity;
        Vector3 moveInput;

        float useGauge = 0.1f;
        float gauge = 100f;
        float maxGauge= 100f;
        
        public event Action onRun;
      
        [SerializeField]
        float jumpPower;
        [SerializeField]
        float runSpeed;
        public float Gauge
        {
            get { return gauge; }
        }
        public float MaxGauge { get { return maxGauge; } }


        void Awake()
        {
            TryGetComponent(out cc);
            TryGetComponent(out animator);
            yaw = player.eulerAngles.y;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
        }

        //잠깐 만들어놓음 나중에 지울것.
        public void UnLockCursor()
        {
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (Input.GetKeyDown(KeyCode.F12))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        public void Move()
        {
            if (Cursor.visible) { return; }
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            moveInput = new Vector3(x, 0f, z);

            float mouseX = Input.GetAxis("Mouse X")*mouseSpeed*Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y")*mouseSpeed*Time.deltaTime;
            
            yaw += mouseX;
            player.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            xRotation -= mouseY;
            xRotation=Mathf.Clamp(xRotation, minX, maxX);
            cameraPivot.localRotation=Quaternion.Euler(xRotation, 0f, 0f);
            
            if (cc.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -1f;
            }

            verticalVelocity += gravity * Time.deltaTime;
            Vector3 moveDir = player.TransformDirection(moveInput);
            Vector3 velocity = moveDir * moveSpeed;
            velocity.y = verticalVelocity;
            cc.Move(velocity * Time.deltaTime);
            speed = moveInput.sqrMagnitude;
            /*
              float speed = moveInput.magnitude;
              animator.SetFloat("Speed", speed);

              if (Input.GetKeyDown(KeyCode.Mouse0))
              {
                  weapon.Attack();
              }
              if (Input.GetKeyDown(KeyCode.E))
              {
                  animator.SetTrigger("Roll");
              }
              if (Input.GetKey(KeyCode.LeftShift))
              {
                  if (gauge <= 0) { gauge = 0;  return; }
                  gauge -= useGauge;
                  moveSpeed = 10f;
                  animator.SetFloat("Speed",2 *speed);
                  onRun?.Invoke();
              }
              else
              {
                  if (gauge >= 100) { gauge = 100; return; }
                  moveSpeed = 5f;
                  gauge += useGauge;
                  onRun?.Invoke();
              }
            */
        }
        public void Attacking()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                weapon.Attack();
            }
        }
        public void Moving()
        {
            animator.SetFloat("Speed", speed);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (gauge <= 0) { gauge = 0; return; }
                gauge -= useGauge;
                moveSpeed = 10f;
                animator.SetFloat("Speed", runSpeed * speed);
                onRun?.Invoke();
            }
           else
            {
                if (gauge >= 100) { gauge = 100; return; }
                moveSpeed = 5f;
                gauge += useGauge;
                onRun?.Invoke();
            }
        }
        /*
        public void Rolling()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
              animator.SetFloat("Roll", speed);
            }
            
        }
        */
       

        public void Jumping()
        {
            if (cc.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -1f;
            }
            if (Input.GetKeyDown(KeyCode.Space)&&cc.isGrounded)
            {
                animator.SetTrigger("Jump");
                verticalVelocity = jumpPower;
            }

        }
        IEnumerator DisapearCo()
        {
            yield return new WaitForSeconds(3f);
            
        }
        void Update()
        {
            Attacking();
           // Rolling();
            Moving();
            Move();
            UnLockCursor();
            Jumping();
        }
    }

}

