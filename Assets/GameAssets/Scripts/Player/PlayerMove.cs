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
        protected float mouseSpeed = 150f;
        [SerializeField]
        protected float minX = -80f;
        [SerializeField]
        protected float maxX = 80f;

        
        
        
        
        [SerializeField]
        Transform player;
        [SerializeField]
        Transform cameraPivot;

        float xRotation = 0f;
        float verticalVelocity;
        Vector3 moveInput;

        void Awake()
        {
            TryGetComponent(out cc);
            TryGetComponent(out animator);
        }
        public void Move()
        {
            
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            moveInput = new Vector3(x, 0f, z);
            float mouseX = Input.GetAxis("Mouse X")*mouseSpeed*Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y")*mouseSpeed*Time.deltaTime;
            player.Rotate(Vector3.up * mouseX);
            xRotation -= mouseY;
            xRotation=Mathf.Clamp(xRotation, minX, maxX);
            cameraPivot.localRotation=Quaternion.Euler(xRotation, 0f, 0f);

            
            if (cc.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -1f;
            }

            verticalVelocity += gravity * Time.deltaTime;
            Vector3 velocity = moveInput * moveSpeed;
            velocity.y = verticalVelocity;
            cc.Move(velocity * Time.deltaTime);
            float speed = moveInput.magnitude;
            animator.SetFloat("Speed", speed);
        }
        void Update()
        {
            Move();
        }
    }

}

