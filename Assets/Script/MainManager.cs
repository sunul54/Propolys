using System.Collections;
using UnityEngine;
using UnityEngine.Audio; // 오디오 믹서 제어용
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // [삭제] public static MainManager Instance 부분 제거

    [Header("커서 설정")]
    public Texture2D cursurIcon;

    [Header("시간 지연 설정")]
    [Tooltip("씬 전환 및 게임 종료 시 지연 시간(초)을 설정합니다.")]
    public float delayTime = 0.5f;

    [Header("오디오 믹서 직접 지정")]
    [Tooltip("프로젝트 창(Project Window)에 있는 GameMixer 파일을 이곳에 드래그해서 넣어주세요.")]
    public AudioMixer audioMixer;

    [Header("게임 데이터 저장")]
    public float globalMouseSensitivity = 200f;
    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;

    // [삭제] void Awake() 내의 싱글톤 검사 및 DontDestroyOnLoad 코드 전체 제거

    void Start()
    {
        Cursor.SetCursor(cursurIcon, Vector2.zero, CursorMode.Auto);
    }

    // ==========================================
    // 오디오 조절 함수들
    // ==========================================
    public void SetBGMVolume(float sliderValue)
    {
        bgmVolume = sliderValue;
        if (audioMixer != null)
        {
            float decibel = sliderValue <= 0.001f ? -80f : Mathf.Log10(sliderValue) * 20f;
            audioMixer.SetFloat("BGM", decibel);
        }
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxVolume = sliderValue;
        if (audioMixer != null)
        {
            float decibel = sliderValue <= 0.001f ? -80f : Mathf.Log10(sliderValue) * 20f;
            audioMixer.SetFloat("SFX", decibel);
        }
    }

    // ==========================================
    // 설정 및 UI 제어 관련 함수들
    // ==========================================
    public void SetMouseSensitivity(float newSensitivity)
    {
        globalMouseSensitivity = newSensitivity;
    }

    public void EnableObject(GameObject target)
    {
        if (target != null) target.SetActive(true);
    }

    public void DisableObject(GameObject target)
    {
        if (target != null) target.SetActive(false);
    }

    public void ToggleObjects(GameObject objectToEnable, GameObject objectToDisable)
    {
        if (objectToEnable != null) objectToEnable.SetActive(true);
        if (objectToDisable != null) objectToDisable.SetActive(false);
    }

    // ==========================================
    // 씬 전환 및 게임 종료 (딜레이 포함)
    // ==========================================
    public void ChangeSceneByName(string sceneName)
    {
        Debug.Log("씬 전환 1 (함수 시작)");
        StartCoroutine(DelayChangeScene(sceneName));
    }

    private IEnumerator DelayChangeScene(string sceneName)
    {
        // [수정] 일시정지 상태(Time.timeScale = 0)에서도 흐르는 현실 시간 기준으로 기다립니다.
        yield return new WaitForSecondsRealtime(delayTime);

        Debug.Log("씬 전환 2 (대기 완료 후 씬 로드)");

        // [추가] 중요! 메인 게임이 일시정지 상태인 채로 타이틀로 가면 타이틀 씬도 멈춰버립니다.
        // 씬을 바꾸기 직전에 게임 속 시간 흐름을 다시 정상(1배속)으로 켜줍니다.
        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        StartCoroutine(DelayQuitGame());
    }

    private IEnumerator DelayQuitGame()
    {
        yield return new WaitForSeconds(delayTime);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}