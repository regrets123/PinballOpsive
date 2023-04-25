using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class AxisToAnimator : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _playerAnim;
    private Gamepad _currentPad;
    private readonly string _animParameter = "leftStickMag";

    void Start()
    {
        _playerAnim = GetComponent<Animator>();
        Assert.IsNotNull(_playerAnim);
        GetPad();
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentPad !=null)
        {
            _playerAnim.SetFloat(_animParameter, _currentPad.leftStick.ReadValue().magnitude);
        }
        else
        {
            GetPad();
        }
    }

    private bool GetPad()
    {
        _currentPad = Gamepad.current;
        if (_currentPad != null)
            return true;
        return false;
    }
}
