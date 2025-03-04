using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCharacter : Character
{
    [SerializeField] private Enemy _target;
    [Inject] private IColliderService _colliderService;
    protected IMovementProvider _playerMovementController;
    

    private bool isInCombat;
    protected override void Init()
    {
        base.Init();
        _playerMovementController = GetComponent<IMovementProvider>();
        _playerMovementController.OnSpeed += OnSpeed_PlayerMovement;
        _playerMovementController.OnSpeedXY += OnSpeed_PlayerMovementXY;
        _playerMovementController.Init();
        _colliderService.SetPlayer(this);
    }
    
    private void OnSpeed_PlayerMovement(float value)
    {
        _animator.SetSpeed(value);
    }

    private void OnSpeed_PlayerMovementXY(Vector2 value)
    {
        _animator.SetSpeedTwoDimension(value);
    }

    private void Update()
    {
        if (Vector3.Distance(_target.transform.position, transform.position) < 5f)
        {
            Debug.Log("Combat");
            isInCombat = true;
            _animator.SetCombat(isInCombat);
            _playerMovementController.SetCombatState(isInCombat);
            _playerMovementController.SetTarget(_target.transform);
        }
        else
        {
            Debug.Log("No Combat");
            isInCombat = false;
            _animator.SetCombat(isInCombat);
            _playerMovementController.SetCombatState(isInCombat);
            _playerMovementController.SetTarget(null);
        }
    }
}
