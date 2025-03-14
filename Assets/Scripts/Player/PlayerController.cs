using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float defaultSpeed;
    public float runSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    public float useStaminaForRun;
    private bool isRun;

    private float jumpDetectionRange = 0.1f;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelte;
    public bool canLook = true;

    private Rigidbody _rigidbody;

    private Animator _animator;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        moveSpeed = defaultSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isRun = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRun)
        {
            if (PlayerManager.Instance.Player.condition.UseStamina(useStaminaForRun * Time.deltaTime))
                moveSpeed = runSpeed;
            else
                moveSpeed = defaultSpeed;
        }
        else
            moveSpeed = defaultSpeed;
        Move();
    }

    private void FixedUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        if (_rigidbody.velocity.magnitude > 0.2f)
            _animator.SetBool("IsMoving", true);
        else
            _animator.SetBool("IsMoving", false);
    }

    void CameraLook()
    {
        camCurXRot += mouseDelte.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        transform.eulerAngles += new Vector3(0, mouseDelte.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelte = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
            return;
        }

        if (context.phase == InputActionPhase.Performed)
        {
            isRun = true;
        }
        
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };


        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], jumpDetectionRange, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
