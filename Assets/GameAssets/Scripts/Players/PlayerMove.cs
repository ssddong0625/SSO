using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMove : MonoBehaviour
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

        [SerializeField]
        Transform player;
        [SerializeField]
        Transform cameraPivot;

        float xRotation = 0f;
        float yaw = 0f;
        float verticalVelocity;
        Vector3 moveInput;

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
        }
        public void Move()
        {
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
            float speed = moveInput.magnitude;
            animator.SetFloat("Speed", speed);
        }
        void Update()
        {
            Move();
            UnLockCursor();
        }
    }

}

