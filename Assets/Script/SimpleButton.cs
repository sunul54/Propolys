using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SimpleButton : MonoBehaviour, IInteractable
{
    [Header("버튼 눌렸을 때 실행할 이벤트")]
    public UnityEvent onButtonActivated;

    [Header("눌림 연출 설정")]
    [SerializeField] private float moveDistance = 0.05f;  // 들어가는 깊이 (미터 단위)
    [SerializeField] private float pressDuration = 0.1f;   // 들어가는 데 걸리는 시간 (초)
    [SerializeField] private float releaseDuration = 0.2f;  // 나오는 데 걸리는 시간 (초)

    private Vector3 initialLocalPosition; // 처음 위치 저장용
    private bool isPressing = false;       // 중복 클릭 방지

    void Start()
    {
        // 시작할 때 버튼의 본래 로컬 위치를 기억해 둡니다.
        initialLocalPosition = transform.localPosition;
    }

    // 인터페이스용 기본 메서드 구현 (방향 정보가 없을 때의 방어 코드)
    public void Interact()
    {
        Interact(Vector3.back);
    }

    // 플레이어가 바라본 방향을 인자로 받아 연출을 처리하는 오버로딩 메서드
    public void Interact(Vector3 pushDirection)
    {
        // 이미 버튼이 작동 중이면 입력 무시
        if (isPressing) return;

        Debug.Log(gameObject.name + " 버튼이 눌렸습니다!");

        // 바라보는 방향을 넘겨주며 코루틴 애니메이션 시작
        StartCoroutine(PressAnimationRoutine(pushDirection));

        // 인스펙터 창에서 연결해 둔 기능(함수)들 실행
        onButtonActivated?.Invoke();
    }

    private IEnumerator PressAnimationRoutine(Vector3 pushDirection)
    {
        isPressing = true;

        // 플레이어의 월드 방향(전체 세상 기준)을 버튼의 로컬 방향(오브젝트 기준)으로 변환합니다.
        // 이 공식 덕분에 버튼이 회전되어 배치되어 있어도 시선 방향으로 밀리게 됩니다.
        Vector3 localPushDirection = transform.InverseTransformDirection(pushDirection).normalized;

        // 들어갔을 때의 최종 목표 위치 계산
        Vector3 pressedPosition = initialLocalPosition + (localPushDirection * moveDistance);

        // [1단계: 들어가기]
        float elapsedTime = 0f;
        while (elapsedTime < pressDuration)
        {
            transform.localPosition = Vector3.Lerp(initialLocalPosition, pressedPosition, elapsedTime / pressDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        transform.localPosition = pressedPosition; // 위치 고정

        // 아주 잠깐 멈춰서 물리적인 반동 느낌 주기
        yield return new WaitForSeconds(0.04f);

        // [2단계: 복귀하기]
        float elapsedTime2 = 0f;
        while (elapsedTime2 < releaseDuration)
        {
            transform.localPosition = Vector3.Lerp(pressedPosition, initialLocalPosition, elapsedTime2 / releaseDuration);
            elapsedTime2 += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialLocalPosition; // 완전히 제자리로 고정

        isPressing = false; // 연출 종료, 다시 누르기 가능
    }
}