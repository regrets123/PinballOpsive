using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Game;
using Opsive.UltimateCharacterController.Objects.CharacterAssist;
using Opsive.UltimateCharacterController.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class DirGravityZone : GravityZone
{
    [SerializeField, Tooltip("The green vector of the transform represent the gravityVector")]
    private Transform _gravityVector;
    [SerializeField]
    private float _fullEffectDelay = 0.4f;
    [SerializeField] 
    protected AnimationCurve m_Influence = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Tooltip("The value to multiply the influence by. A larger value can be used for larger spheres.")]
    private float _timeInsideZone;
    private bool _insideZone;

    private void Start()
    {
        Assert.IsNotNull(_gravityVector, gameObject.name + " is missing a gravityVector transform");
    }

    private void OnTriggerStay(Collider other)
    {
        // A main character collider is required.
        if (!MathUtility.InLayerMask(other.gameObject.layer, 1 << LayerManager.Character))
        {
            return;
        }
        _insideZone = true;
        _timeInsideZone += Time.deltaTime;
    }

    private void OnTriggerExit(Collider other)
    {
        _insideZone = false;
        var characterLocomotion = other.GetComponentInParent<UltimateCharacterLocomotion>();
        if (characterLocomotion != null)
        {
            var alignToGravity = characterLocomotion.GetAbility<AlignToGravityZone>();
            if (alignToGravity != null)
            {
                StartCoroutine(LingeringGravity(alignToGravity));
            }
        }     
    }

    public override Vector3 DetermineGravityDirection(Vector3 position)
    {
        var influenceFactor = m_Influence.Evaluate(EvaluateTimeInside());
        Vector3 transformPos = _gravityVector.transform.up *-1f;
        Debug.Log("Gravity influence is " + influenceFactor);
        return transformPos * influenceFactor;
    }

    private IEnumerator LingeringGravity(AlignToGravityZone zone)
    {
        while (!_insideZone && _timeInsideZone > 0)
        {
            _timeInsideZone -= Time.deltaTime;
            yield return null;
        }
        zone.UnregisterGravityZone(this);
        _timeInsideZone = 0;
    }

    private float EvaluateTimeInside()
    {
        if (_timeInsideZone > _fullEffectDelay)
        {
            return 1;
        }
        if(_timeInsideZone <= 0)
        {
            return 0;
        }
        return _timeInsideZone / _fullEffectDelay;
    }
}
