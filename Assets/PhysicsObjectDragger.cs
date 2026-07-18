using UnityEngine;

public class PhysicsObjectDragger : MonoBehaviour
{
    [Header("드래그 설정")]
    // 태그 이름 (유니티 에디터에서 물체에 부여한 태그와 일치해야 합니다)
    public string draggableTag = "Draggable";
    // 카메라로부터 물체를 정확히 몇 미터 앞에 띄워둘지 설정
    public float holdDistance = 3.0f;

    private GameObject heldObject;
    private Rigidbody heldRigidbody;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 마우스 클릭 및 떼기 입력 감지
        if (Input.GetMouseButtonDown(0))
        {
            TryPickupObject();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DropObject();
        }

        // 물체를 들고 있는 동안 실시간 위치 고정 처리
        if (heldObject != null)
        {
            KeepObjectInFrontOfCamera();
        }
    }

    void TryPickupObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.collider.CompareTag(draggableTag))
            {
                heldObject = hit.collider.gameObject;
                heldRigidbody = heldObject.GetComponent<Rigidbody>();

                if (heldRigidbody != null)
                {
                    // 바닥에 잠들어 있던 물리 연산을 깨웁니다.
                    if (heldRigidbody.IsSleeping())
                    {
                        heldRigidbody.WakeUp();
                    }

                    // [핵심] 들고 있는 동안은 물리 충돌 및 물리 엔진 영향을 완전히 끕니다.
                    // 이렇게 해야 다른 장애물에 걸리거나 미끄러져 흔들리지 않습니다.
                    heldRigidbody.isKinematic = true;
                    heldRigidbody.useGravity = false;
                }
            }
        }
    }

    void KeepObjectInFrontOfCamera()
    {
        // 마우스 화면 좌표를 기반으로 월드 좌표계 계산 (Z축 거리는 holdDistance 만큼)
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = holdDistance;
        Vector3 targetPosition = mainCamera.ScreenToWorldPoint(mousePos);

        // 1. 물체의 위치를 마우스가 지리키는 화면 바로 앞 좌표로 순간이동 시킵니다.
        heldObject.transform.position = targetPosition;

        // 2. 물체가 회전하지 않고 카메라 정면을 똑바로 바라보도록 고정합니다.
        // 만약 물체 고유의 원래 회전(0,0,0)을 유지하고 싶다면 heldObject.transform.rotation = Quaternion.identity; 로 바꾸셔도 됩니다.
        heldObject.transform.rotation = mainCamera.transform.rotation;
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            if (heldRigidbody != null)
            {
                // [핵심] 물체를 놓는 순간 물리 엔진(중력 등)을 다시 켜서 저울 위로 툭 떨어지게 만듭니다.
                heldRigidbody.isKinematic = false;
                heldRigidbody.useGravity = true;
            }

            heldObject = null;
            heldRigidbody = null;
        }
    }
}