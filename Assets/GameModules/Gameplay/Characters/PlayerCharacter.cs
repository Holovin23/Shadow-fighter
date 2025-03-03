using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCharacter : Character
{
    [Inject] private IColliderService _colliderService;
    protected IMovementProvider _playerMovementController;

    protected override void Init()
    {
        base.Init();
        _playerMovementController = GetComponent<IMovementProvider>();
        _playerMovementController.OnSpeed += OnSpeed_PlayerMovement;
        _playerMovementController.Init();
        _colliderService.SetPlayer(this);
    }
    
    private void OnSpeed_PlayerMovement(float value)
    {
        _animator.SetSpeed(value);
    }
}
