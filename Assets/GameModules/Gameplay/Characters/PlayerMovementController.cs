using System;
using UnityEngine;
using Zenject;

public class PlayerMovementController : MonoBehaviour, IMovementProvider
{
    [Inject] public Joystick joystick;        // Ссылка на джойстик

    public float speed = 5f;         // Максимальная скорость передвижения
    public float acceleration = 10f; // Скорость разгона
    public float deceleration = 5f;  // Скорость торможения
    public float gravity = 9.81f;    // Гравитация
    public float rotationSpeed = 10f;// Скорость поворота

    private CharacterController controller;
    private Vector3 velocity;
    private float verticalVelocity = 0f;

    public event Action<float> OnSpeed;
    public event Action<Vector2> OnSpeedXY;

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

        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 targetVelocity = moveDirection * speed;
            velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * acceleration);

            // Поворот в сторону движения
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }
        else
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * deceleration);
        }

        Vector3 finalMove = velocity * Time.deltaTime;
        finalMove.y = verticalVelocity * Time.deltaTime;

        controller.Move(finalMove);
        
        float speedValue = velocity.magnitude / speed;
        OnSpeed?.Invoke(speedValue);
    }
}