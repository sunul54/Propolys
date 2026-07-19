using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("일시정지 UI 설정")]
    [Tooltip("ESC를 눌렀을 때 켜고 끌 일시정지 패널(UI 오브젝트)을 드래그해 넣으세요.")]
    public GameObject pauseMenuPanel;

    [Tooltip("게임 중에 항상 켜져 있던 기본 UI(예: HUD, 크로스헤어 등)를 드래그해 넣으세요.")]
    public GameObject inGameHUDPanel;

    private bool isPaused = false; // 현재 일시정지 상태인지 체크

    void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (inGameHUDPanel != null) inGameHUDPanel.SetActive(true);

        // 게임 시작 시 기본적으로 커서를 숨깁니다.
        ApplyCursorState(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (inGameHUDPanel != null) inGameHUDPanel.SetActive(false);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);

            // 일시정지 상태: 커서 보이게 설정
            ApplyCursorState(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (inGameHUDPanel != null) inGameHUDPanel.SetActive(true);

            // 재개 상태: 커서 확실하게 숨기기
            ApplyCursorState(false);
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (inGameHUDPanel != null) inGameHUDPanel.SetActive(true);

        // 버튼 클릭으로 재개 시에도 커서 확실하게 숨기기
        ApplyCursorState(false);
    }

    // 커서 상태를 강제로 고정하고 제어하는 별도 함수
    private void ApplyCursorState(bool showCursor)
    {
        if (showCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // 커서를 화면 중앙에 완벽히 가두고 안 보이게 처리합니다.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // 유니티 창을 내렸다가 다시 올렸을 때 커서가 풀리는 현상 방지
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && !isPaused)
        {
            ApplyCursorState(false);
        }
    }
}