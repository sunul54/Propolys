using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverAlpha : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image targetImage;

    void Awake()
    {
        // 오브젝트에 연결된 Image 컴포넌트를 가져옵니다.
        targetImage = GetComponent<Image>();

        // 시작할 때는 마우스가 밖에 있으므로 투명도를 0으로 설정합니다.
        SetAlpha(0f);
    }

    // 마우스가 버튼 영역으로 들어올 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAlpha(1f); // 투명도 100% (불투명)
    }

    // 마우스가 버튼 영역에서 나갈 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        SetAlpha(0f); // 투명도 0% (완전 투명)
    }

    // 투명도를 변경하는 헬퍼 함수
    private void SetAlpha(float alpha)
    {
        if (targetImage != null)
        {
            Color color = targetImage.color;
            color.a = alpha;
            targetImage.color = color;
        }
    }
}