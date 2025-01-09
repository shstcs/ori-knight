using UnityEngine;

public class PlayerTrackCamera : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾��� Transform
    public Vector2 deadZoneSize = new Vector2(5.0f, 3.0f); // �������� ũ��
    public float smoothSpeed = 0.01f; // ī�޶��� �ε巯�� �̵� �ӵ�
    public Vector2 minBound = new Vector2(0, 0);
    public Vector2 maxBound = new Vector2(83.6f, 29.6f);

    private Vector3 threshold;

    void Start()
    {
        // �ʱ� ������ �Ӱ�ġ�� �����մϴ�.
        threshold = new Vector3(deadZoneSize.x / 2, deadZoneSize.y / 2, 0);
    }

    void LateUpdate()
    {
        // ī�޶�� �÷��̾��� �������� ���
        Vector3 offset = playerTransform.position - transform.position;

        // ���� �������� ������ �Ӱ�ġ���� ũ�ٸ�, ī�޶� �̵�
        if (Mathf.Abs(offset.x) > threshold.x || Mathf.Abs(offset.y) > threshold.y)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, playerTransform.position, smoothSpeed);
            float clampedX = Mathf.Clamp(smoothedPosition.x, minBound.x, maxBound.x);
            float clampedY = Mathf.Clamp(smoothedPosition.y, minBound.y, maxBound.y);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
