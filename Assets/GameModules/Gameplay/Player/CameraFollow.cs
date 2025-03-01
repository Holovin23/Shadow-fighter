using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // Игрок (цель для слежения)
    public Vector3 offset = new Vector3(0, 10, -5); // Смещение камеры
    public float followSpeed = 5f;   // Скорость следования

    void LateUpdate()
    {
        if (target == null) return;

        // Целевая позиция камеры
        Vector3 targetPosition = target.position + offset;

        // Плавное движение камеры
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        // Смотрит вниз (для вида сверху)
        transform.LookAt(target.position);
    }
}