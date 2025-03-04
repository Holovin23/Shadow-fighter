using System;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Character : MonoBehaviour
{
    protected IAnimatorProvider _animator;

    
    private void Start()
    {
        Init();
    }
    
    protected virtual void Init()
    {
        _animator = GetComponentInChildren<IAnimatorProvider>();
    }
    
    
}
