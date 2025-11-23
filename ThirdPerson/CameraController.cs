using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float sensitivity = 100f;
    public float distance;
    public float height;
    public float verticalAngle;
    private float cameraRotationY;
    public float dynamic_distance;
    public LayerMask wallLayer;
    public float wall_distance;
    public Camera cam;
    public LayerMask currentMask;
    public float xRotation;
    public float yRotation;
    void Start()
    {
        xRotation = 0; yRotation = 0;
        currentMask = cam.cullingMask;
        verticalAngle = 15;
        height = 1.5f;
        currentMask |= (1 << LayerMask.NameToLayer("Player")); // Ĭ��������Ҳ�
        cam.cullingMask = currentMask;
        UpdateCameraPosition();
    }

    void Update()
    {
        cameraRotationY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        verticalAngle -= Input.GetAxis("Mouse Y") * sensitivity * 0.5f * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, -10f, 30f); // ���ƴ�ֱ�Ƕȷ�Χ
        dynamic_distance = Mathf.Abs(verticalAngle) / 40;
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        distance = 2.5f + dynamic_distance;
        RaycastHit hit;
        bool touch = Physics.Raycast(player.position + Vector3.up * height, -(Quaternion.Euler(verticalAngle, cameraRotationY, 0) * Vector3.forward), out hit, distance, wallLayer);
        if (touch)
        {
            distance = Mathf.Max(hit.distance - 0.1f, 0.1f);
        }
        else
        {
            distance = Mathf.Lerp(distance, 2f + dynamic_distance, 0.5f);
        }
        Quaternion cameraRotation = Quaternion.Euler(verticalAngle, cameraRotationY, 0);
        Vector3 cameraOffset = cameraRotation * new Vector3(0, height, -distance);
        Vector3 targetPosition = player.position + cameraOffset;
        transform.position = targetPosition;
        transform.LookAt(player.position + Vector3.up * height);
    }
}
