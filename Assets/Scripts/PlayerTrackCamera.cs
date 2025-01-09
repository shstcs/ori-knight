using UnityEngine;

public class PlayerTrackCamera : MonoBehaviour
{
    public Transform playerTransform; // 플레이어의 Transform
    public Vector2 deadZoneSize = new Vector2(5.0f, 3.0f); // 데드존의 크기
    public float smoothSpeed = 0.01f; // 카메라의 부드러운 이동 속도
    public Vector2 minBound = new Vector2(0, 0);
    public Vector2 maxBound = new Vector2(83.6f, 29.6f);

    private Vector3 threshold;

    void Start()
    {
        // 초기 데드존 임계치를 설정합니다.
        threshold = new Vector3(deadZoneSize.x / 2, deadZoneSize.y / 2, 0);
    }

    void LateUpdate()
    {
        // 카메라와 플레이어의 오프셋을 계산
        Vector3 offset = playerTransform.position - transform.position;

        // 만약 오프셋이 데드존 임계치보다 크다면, 카메라를 이동
        if (Mathf.Abs(offset.x) > threshold.x || Mathf.Abs(offset.y) > threshold.y)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, playerTransform.position, smoothSpeed);
            float clampedX = Mathf.Clamp(smoothedPosition.x, minBound.x, maxBound.x);
            float clampedY = Mathf.Clamp(smoothedPosition.y, minBound.y, maxBound.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
