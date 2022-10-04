using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInputSystem : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.1f;
    PinballOpsiveInputSystem playerInputActions;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerInputActions = new PinballOpsiveInputSystem();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
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
        }
    }
}
