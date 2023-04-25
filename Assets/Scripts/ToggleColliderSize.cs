using UnityEngine;
using UnityEngine.Assertions;
using Opsive.UltimateCharacterController.Character.Abilities;

public class ToggleColliderSize : MonoBehaviour
{
    [SerializeField]
    private int _indexParameter;
    [SerializeField]
    private float _heightDuringActive;
    [SerializeField]
    private float _heightDuringInactive;
    [SerializeField]
    private CapsuleCollider _collider;

    private void Start()
    {
        Assert.IsNotNull(_collider);
    }

    public void ToggleCollider(Ability ability, bool activated)
    {
        if(ability.AbilityIndexParameter == _indexParameter)
        {
            _collider.height = activated ? _heightDuringActive : _heightDuringInactive;
        }
        
    }
}
