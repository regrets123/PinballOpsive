using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInputSystem : MonoBehaviour
{
    [SerializeField]
    private float _speed = 0.1f;
    PinballOpsiveInputSystem _playerInputActions;
    private Animator _animator;
    private bool _grounded;


    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _playerInputActions = new PinballOpsiveInputSystem();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Jump.performed += Jump;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        Debug.Log("inputMag is " + inputVector.magnitude);
        _animator.SetFloat("sideMovement", inputVector.x, .1f, Time.deltaTime);
        _animator.SetFloat("forwardMovement", inputVector.y, .1f, Time.deltaTime);
        _animator.SetFloat("Magnitude", inputVector.magnitude, .1f, Time.deltaTime);
    }

    public void SetGrounded(bool isGrounded)
    {
        _grounded = isGrounded;
        _animator.SetBool("Grounded", isGrounded);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (context.performed && _grounded)
        {
            _animator.SetTrigger("Jump");
            Debug.Log("Jump " + context.phase);
        }
    }
}
