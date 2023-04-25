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
    [SerializeField]
    private int _indexParameter; 


    private void Start()
    {
        Assert.IsNotNull(_anim);
        Assert.IsTrue(_motions.Length == 2);
        Assert.IsTrue(_motions.Length == _keyfloatThresholds.Length);
    }


    public void SwitchMotion(Ability ability, bool activated)
    {
        if(activated && ability.AbilityIndexParameter == _indexParameter)
        {
            float speedThreshhold = _anim.GetFloat(_animKeyFloat);
            for (int i = 0; i < _keyfloatThresholds.Length; i++)
            {
                if (_keyfloatThresholds[i] < speedThreshhold)
                {
                    ability.AnimatorMotion = _motions[i];                
                    Debug.Log("Motion on quickstop is now " + ability.AnimatorMotion.name);
                    break;
                }
            }           
        }    
    }
}
