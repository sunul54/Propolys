using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 설정")]
    [SerializeField] private float interactRange = 5.0f; // 상호작용 가능 거리 (기존보다 늘림)
    [SerializeField] private LayerMask interactableLayer = ~0; // 기본값 Everything

    private Transform cam;
    private GameObject lastLookedObject = null; // 직전에 바라보던 오브젝트 저장용

    void Start()
    {
        // 메인 카메라 캐싱
        cam = Camera.main.transform;
    }

    void Update()
    {
        // 씬 뷰에서 조준점 레이저 디버그 라인 그리기 (빨간색)
        Debug.DrawRay(cam.position, cam.forward * interactRange, Color.red);

        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        // 레이캐스트가 무언가에 부딪혔고, 그 오브젝트의 태그가 "Button"인 경우
        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer) && hit.collider.CompareTag("Button"))
        {
            GameObject currentObject = hit.collider.gameObject;

            // [조준 시작 감지] 이전과 다른 버튼이거나, 새로 버튼을 바라봤을 때 딱 한 번 실행
            if (currentObject != lastLookedObject)
            {
                Debug.Log($"[조준 시작] 현재 '{currentObject.name}' 버튼을 바라보고 있습니다. (F키로 상호작용 가능)");
                lastLookedObject = currentObject;
            }

            // F 키 입력 감지
            if (Input.GetMouseButtonDown(0))
            {
                // 먼저 플레이어가 바라보는 방향을 쓸 수 있는 SimpleButton인지 확인
                SimpleButton button = currentObject.GetComponent<SimpleButton>();

                if (button != null)
                {
                    // 카메라의 정면 방향(cam.forward)을 함께 전달
                    button.Interact(cam.forward);
                }
                else
                {
                    // 방향 정보가 필요 없는 일반 인터페이스 기반 상호작용 처리용
                    IInteractable interactable = currentObject.GetComponent<IInteractable>();
                    interactable?.Interact();
                }
            }
        }
        else
        {
            // [조준 해제 감지] 직전까지 버튼을 보고 있었다면 딱 한 번 실행
            if (lastLookedObject != null)
            {
                Debug.Log($"[조준 해제] '{lastLookedObject.name}' 버튼에서 조준을 벗어났습니다.");
                lastLookedObject = null;
            }
        }
    }
}