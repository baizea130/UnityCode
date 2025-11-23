using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public GameObject cameraDir;
    public Animator animator;
    [SerializeField] Camera cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        speed = 3;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDir = cameraDir.transform.forward * vertical + cameraDir.transform.right * horizontal;
        moveDir = moveDir.normalized * speed * Time.deltaTime;

        rb.MovePosition(rb.position + moveDir);

        if (moveDir != Vector3.zero && transform.position.y < 1)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime * 10);
            animator.SetBool("Run", true);
            rb.freezeRotation = true;
        }
    }
}
