using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("메인 매니저 연결")]
    [Tooltip("현재 씬 하이어라키에 있는 MainManager 오브젝트를 여기에 드래그하세요. 비워두면 자동으로 찾습니다.")]
    public MainManager mainManager;

    [Header("카메라 설정")]
    public float mouseSensitivity = 200f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        // 1. 만약 인스펙터 창에서 깜빡하고 드래그를 안 했다면, 자동으로 씬에서 검색해서 찾아옵니다.
        if (mainManager == null)
        {
            mainManager = Object.FindFirstObjectByType<MainManager>();
        }

        // 2. 메인 매니저를 찾았다면 저장된 감도 값을 가져옵니다.
        if (mainManager != null)
        {
            mouseSensitivity = mainManager.globalMouseSensitivity;
        }

        // 초기 커서 세팅
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ★ 중요: 일시정지 상태(시간 배속이 0일 때)라면 마우스로 시점이 회전하지 않도록 작동을 막습니다.
        if (Time.timeScale == 0f) return;

        // 실시간으로 메인 매니저의 슬라이더 감도 값을 동기화합니다.
        if (mainManager != null)
        {
            mouseSensitivity = mainManager.globalMouseSensitivity;
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 카메라 위아래 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 플레이어 좌우 회전
        playerBody.Rotate(Vector3.up * mouseX);
    }
}