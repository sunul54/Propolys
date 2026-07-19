using UnityEngine;

public class UIRotator : MonoBehaviour
{
    public enum RotationDirection
    {
        Clockwise,        // 시계 방향
        CounterClockwise  // 시계 반대 방향
    }

    [Header("2D UI 회전 설정")]
    [Tooltip("회전 방향을 선택하세요.")]
    public RotationDirection direction = RotationDirection.Clockwise;

    [Tooltip("회전 속도입니다. 슬라이더를 움직여 조절하세요.")]
    [Range(0f, 720f)]
    public float rotationSpeed = 90f; // 초당 회전 각도

    void Update()
    {
        // 시계 방향은 유니티 내부 각도 계산상 마이너스(-) 방향입니다.
        float angleSign = (direction == RotationDirection.Clockwise) ? -1f : 1f;

        // 2D UI는 오직 Z축(Vector3.forward)을 기준으로만 회전합니다.
        transform.Rotate(Vector3.forward * angleSign * rotationSpeed * Time.deltaTime);
    }
}