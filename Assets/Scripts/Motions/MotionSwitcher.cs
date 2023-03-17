using UnityEngine;
using UnityEngine.Assertions;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.Motion;

public class MotionSwitcher : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;
    [SerializeField]
    private AnimatorMotion[] _motions;
    [SerializeField]
    private float[] _keyfloatThresholds;
    [SerializeField]
    private string _animKeyFloat = "ForwardMovement";


    private void Start()
    {
        Assert.IsNotNull(_anim);
        Assert.IsTrue(_motions.Length == 2);
        Assert.IsTrue(_motions.Length == _keyfloatThresholds.Length);
    }


    public void SwitchMotion(Ability ability, bool activated)
    {
        if(activated)
        {
            bool newSpeed = false;
            float speedThreshhold = _anim.GetFloat(_animKeyFloat);
            for (int i = 0; i < _keyfloatThresholds.Length; i++)
            {
                if (_keyfloatThresholds[i] < speedThreshhold)
                {
                    newSpeed = true;
                    ability.AnimatorMotion = _motions[i];                
                    Debug.Log("Motion on quickstop is now " + ability.AnimatorMotion.name);
                    break;
                }
            }
            if(!newSpeed)
            {
                ability.AnimatorMotion = _motions[_motions.Length-1];
                Debug.Log("Motion on quickstop is lowestSpeed " + ability.AnimatorMotion.name);
            }
        }    
    }
}
