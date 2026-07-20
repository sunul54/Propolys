using UnityEngine;
using System.Collections;

public class TargetRotator : MonoBehaviour
{
    [Header("회전 설정")]
    [SerializeField] private float rotationDuration = 3.0f; // 회전 시간(초)

    private float initialYAngle; // 시작할 때의 초기 Y축 각도 저장용
    private Coroutine rotateRoutine;

    void Start()
    {
        // 시작할 때 이 오브젝트의 로컬 오일러 각도 중 'Y값'만 정확히 기억합니다.
        initialYAngle = transform.localEulerAngles.y;
    }

    // 유니티 인스펙터 버튼 이벤트(Static int)에 연결할 함수 (0~11 입력)
    public void RotateToStep(int stepIndex)
    {
        // 12개 분할 기준 한 칸당 30도씩 계산
        float targetYAngle = initialYAngle + (stepIndex * 30f);

        // 360도를 넘어갈 때 각도가 꼬이는 것을 방지하기 위해 정규화 수행
        targetYAngle = targetYAngle % 360f;

        // 기존 회전 연출이 돌고 있다면 멈추고 새로 시작
        if (rotateRoutine != null) StopCoroutine(rotateRoutine);
        rotateRoutine = StartCoroutine(RotateYRoutine(targetYAngle));
    }

    private IEnumerator RotateYRoutine(float targetYAngle)
    {
        // 현재 오브젝트의 로컬 오일러 각도(X, Y, Z)를 가져옵니다.
        Vector3 currentAngles = transform.localEulerAngles;

        float startYAngle = currentAngles.y;
        float elapsedTime = 0f;

        // 보정: 유니티 오일러 특성상 마이너스 각도나 오차가 생길 수 있어 최단 경로(LerpAngle) 계산용 세팅
        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            t = Mathf.SmoothStep(0f, 1f, t); // 부드러운 감속

            // Mathf.LerpAngle을 사용하면 350도에서 10도로 돌 때 엉뚱한 방향으로 한 바퀴 돌지 않고 최단 거리로 회전합니다.
            float currentY = Mathf.LerpAngle(startYAngle, targetYAngle, t);

            // 다른 X, Z축 값은 현재 오브젝트가 가진 고유 값을 그대로 유지하고, Y축 값만 교체합니다.
            transform.localEulerAngles = new Vector3(currentAngles.x, currentY, currentAngles.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 프레임에 목표 Y축 각도로 완벽하게 고정
        transform.localEulerAngles = new Vector3(currentAngles.x, targetYAngle, currentAngles.z);
    }
}