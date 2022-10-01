using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInputSystem : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.1f;
    Rigidbody characterRigidBody;
    PinballOpsiveInputSystem playerInputActions;
    private Animator animator;


    private void Awake()
    {
        characterRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        playerInputActions = new PinballOpsiveInputSystem();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        Vector3 newPosition = characterRigidBody.position + new Vector3(inputVector.x, 0, inputVector.y) * speed;
        characterRigidBody.MovePosition(newPosition);
        animator.SetFloat("sideMovement", inputVector.x, .1f, Time.deltaTime);
        animator.SetFloat("forwardMovement", inputVector.y, .1f, Time.deltaTime);
    }

    private void Update()
    {
        //Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        //Debug.Log(inputVector);
        //animator.SetFloat("sideMovement", inputVector.x, .1f, Time.deltaTime);
        //animator.SetFloat("forwardMovement", inputVector.y, .1f, Time.deltaTime);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (context.performed)
        {
            Debug.Log("Jump " + context.phase);
            characterRigidBody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
