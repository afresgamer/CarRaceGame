using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Header("車両情報")]
    private CarInfo carInfo;
    [SerializeField, Header("最大速度"), Range(180f, 400f)]
    private float maxMoveSpeed = 200.0f;
    [SerializeField, Header("最大回転角度"), Range(15f, 45f)]
    private float maxSpinAngle = 45.0f;

    Rigidbody rb;
    CarInputSystem carInputSystem;
    Vector2 inputValue;

    private const float CONST_SPEED = 10000;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 車両情報を設定
        rb.mass = carInfo.weight;
        rb.drag = carInfo.drag;
        rb.angularDrag = carInfo.angularDrag;

        carInputSystem = new CarInputSystem();
        carInputSystem.CarPlayer.Move.started += OnMove;
        carInputSystem.CarPlayer.Move.performed += OnMove;
        carInputSystem.CarPlayer.Move.canceled += OnMove;

        carInputSystem.Enable();
    }

    void Update()
    {
        if (!CarGameManager.Instance.IsGameStart) return;

        Movement();
    }

    void Movement()
    {
        if (CarGameManager.Instance.IsGoal) return;

        // 速度制限
        float sqrVel = rb.velocity.sqrMagnitude;
        if (sqrVel > maxMoveSpeed) return;
        // 移動
        rb.AddForce(transform.forward * inputValue.y * carInfo.moveSpeed * CONST_SPEED * Time.deltaTime, ForceMode.Force);
        
        // 回転制限
        float sqrAng = rb.angularVelocity.sqrMagnitude;
        if (sqrAng > maxSpinAngle) return;
        // 回転
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += inputValue.x * carInfo.spinSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(rot);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputValue = context.ReadValue<Vector2>();
    }

    private void OnDisable() 
    {
        carInputSystem.CarPlayer.Move.started -= OnMove;
        carInputSystem.CarPlayer.Move.performed -= OnMove;
        carInputSystem.CarPlayer.Move.canceled -= OnMove;

        carInputSystem?.Disable();
    }

    private void OnDestroy() 
    {
        carInputSystem?.Dispose();
    }
}
