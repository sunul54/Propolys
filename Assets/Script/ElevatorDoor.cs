using UnityEngine;
using System.Collections;

public class ElevatorDoor : MonoBehaviour
{
    [Header("문 움직임 설정")]
    // 로컬 축 기준 어느 방향으로 열릴지 설정 (예: 우측으로 열리면 Vector3.right)
    [SerializeField] private Vector3 openDirection = Vector3.right;
    // 이동할 거리 (미터 단위)
    [SerializeField] private float moveDistance = 1.2f;

    [Header("시간 설정 (초)")]
    [SerializeField] private float openDuration = 1.5f;  // 열리는 데 걸리는 시간
    [SerializeField] private float closeDuration = 1.5f; // 닫히는 데 걸리는 시간
    [SerializeField] private float stayOpenTime = 3.0f;  // 열린 채로 유지되는 시간

    private Vector3 closedPosition; // 닫힌 상태 (처음 위치)
    private Vector3 openPosition;   // 열린 상태 위치
    private Coroutine doorRoutine;  // 실행 중인 코루틴 제어용
    private bool isOpen = false;    // 현재 문 상태

    void Start()
    {
        // 시작할 때의 문 위치를 닫힌 상태의 기준으로 기억합니다.
        closedPosition = transform.localPosition;
        // 열렸을 때의 목표 위치를 계산합니다.
        openPosition = closedPosition + (openDirection.normalized * moveDistance);
    }

    // [버튼 연결용 메서드] 문을 열고 일정 시간 뒤 자동으로 닫히게 합니다.
    public void OpenAndAutoClose()
    {
        if (doorRoutine != null) StopCoroutine(doorRoutine);
        doorRoutine = StartCoroutine(OpenAndAutoCloseRoutine());
    }

    // [수동 제어용] 문을 열기만 하는 기능
    public void OpenDoor()
    {
        if (doorRoutine != null) StopCoroutine(doorRoutine);
        doorRoutine = StartCoroutine(MoveDoor(openPosition, openDuration));
        isOpen = true;
    }

    // [수동 제어용] 문을 닫기만 하는 기능
    public void CloseDoor()
    {
        if (doorRoutine != null) StopCoroutine(doorRoutine);
        doorRoutine = StartCoroutine(MoveDoor(closedPosition, closeDuration));
        isOpen = false;
    }

    // 자동으로 열렸다 닫히는 코루틴
    private IEnumerator OpenAndAutoCloseRoutine()
    {
        // 1. 문 열기
        yield return StartCoroutine(MoveDoor(openPosition, openDuration));
        isOpen = true;

        // 2. 대기 시간
        yield return new WaitForSeconds(stayOpenTime);

        // 3. 문 닫기
        yield return StartCoroutine(MoveDoor(closedPosition, closeDuration));
        isOpen = false;
    }

    // 문을 부드럽게 이동시키는 공용 코루틴
    private IEnumerator MoveDoor(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp를 이용해 부드럽게 이동 (SmoothStep을 섞어 가속/감속 효과 추가)
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0f, 1f, t); // 문이 열리고 닫힐 때 부드러운 느낌 제공

            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition; // 정확한 목적지 고정
    }
}