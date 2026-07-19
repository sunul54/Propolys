using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public Texture2D cursurIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(cursurIcon, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때 플레이 모드를 종료합니다.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 실제 게임(PC, 모바일 등)을 완전히 종료합니다.
        Application.Quit();
#endif
    }
}
