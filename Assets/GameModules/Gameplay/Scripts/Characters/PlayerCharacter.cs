using System;
using System.Collections;
using System.Collections.Generic;
using GameModules.Gameplay.Scripts.Characters.Damage;
using GameModules.Gameplay.Scripts.Characters.Health;
using GameModules.Gameplay.Scripts.Characters.Stats;
using UnityEngine;
using Zenject;

public class PlayerCharacter : Character
{    
    [SerializeField] private Enemy _target; //TODO remove
    
    [Inject] private IColliderService _colliderService;
    [SerializeField] private PlayerMovementController _playerMovementController;
    
    private bool isInCombat;
    
    public IMovementProvider PlayerMovementController => _playerMovementController;

    protected override void Init()
    {
        base.Init();
        _playerMovementController.OnSpeed += OnSpeed_PlayerMovement;
        _playerMovementController.OnSpeedXY += OnSpeed_PlayerMovementXY;
        _playerMovementController.Init();
        _colliderService.SetPlayer(this);
        Debug.Log("Inited");
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
        /*if (Vector3.Distance(_target.transform.position, transform.position) < 5f)
        {
            isInCombat = true;
            _animator.SetCombat(isInCombat);
            _playerMovementController.SetCombatState(isInCombat);
            _playerMovementController.SetTarget(_target.transform);
        }
        else
        {
            isInCombat = false;
            _animator.SetCombat(isInCombat);
            _playerMovementController.SetCombatState(isInCombat);
            _playerMovementController.SetTarget(null);
        }*/
    }

}
