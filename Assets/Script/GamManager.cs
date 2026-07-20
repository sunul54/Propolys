using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    private Rigidbody rb;

    private float hInput;
    private float vInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // 코드상에서도 물리 보간을 확실하게 켜줍니다.
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        // 1. 키보드 입력은 누락 없이 받아오기 위해 매 프레임 실행되는 Update에서 수행합니다.
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // 2. 리지드바디를 조작하는 물리 이동은 고정 프레임 주기인 FixedUpdate에서 수행합니다.
        Move();
    }

    void Move()
    {
        Vector3 moveDirection = new Vector3(hInput, 0, vInput).normalized;

        // 유니티 6 권장 linearVelocity 설정
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }
    }
}