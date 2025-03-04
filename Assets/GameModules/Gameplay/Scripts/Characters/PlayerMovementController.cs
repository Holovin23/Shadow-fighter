using System;
using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour, IMovementProvider
{
    [Inject] public Joystick joystick; // Ссылка на джойстик

    public float speed = 5f;         // Максимальная скорость передвижения
    public float acceleration = 10f; // Скорость разгона
    public float deceleration = 5f;  // Скорость торможения
    public float gravity = 9.81f;    // Гравитация
    public float rotationSpeed = 10f;// Скорость поворота
    public bool _isRanged;

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalVelocity = 0f;
    
    private bool _isInCombat; // В бою
    private Transform _target; // Цель для поворота (если _isRanged)

    public event Action<float> OnSpeed;   // Скорость от 0 до 1
    public event Action<Vector2> OnSpeedXY; // Скорость в 2D

    public void Init()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyGravity();
        MovePlayer();
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f; // Маленькое значение, чтобы CharacterController понимал, что на земле
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    private void MovePlayer()
    {
        Vector2 inputDirection = joystick.Direction;
        Vector3 moveDirection = new Vector3(inputDirection.x, 0, inputDirection.y);

        if (_isRanged)
        {
            if (_isInCombat && _target != null)
            {
                // Если в бою и дальник, всегда смотрим на цель
                Vector3 lookDirection = _target.position - transform.position;
                lookDirection.y = 0; // Убираем наклон вверх/вниз

                if (lookDirection.magnitude > 0.1f)
                {
                    transform.forward = Vector3.Lerp(transform.forward, lookDirection.normalized, Time.deltaTime * rotationSpeed);
                }
            }
            else if (moveDirection.magnitude > 0.1f)
            {
                // Если НЕ в бою — дальник поворачивается по направлению движения
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            // Ближник всегда поворачивается по направлению движения
            if (moveDirection.magnitude > 0.1f)
            {
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
            }
        }

        // Движение игрока
        Vector3 targetVelocity = moveDirection * speed;
        velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * acceleration);

        Vector3 finalMove = velocity * Time.deltaTime;
        finalMove.y = verticalVelocity * Time.deltaTime;
        controller.Move(finalMove);

        // Передаем скорость в аниматор
        float speedValue = velocity.magnitude / speed;
        OnSpeed?.Invoke(speedValue);
        OnSpeedXY?.Invoke(new Vector2(velocity.x / speed, velocity.z / speed));
    }

    public void SetCombatState(bool isInCombat)
    {
        _isInCombat = isInCombat;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
