using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float runSpeed = 8.0f;
    [SerializeField] private float gravity = -19.62f; // Gravedad acelerada para mejor sensación
    [SerializeField] private float jumpHeight = 1.2f;

    [Header("Cámara y Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float topClamp = -89.0f;
    [SerializeField] private float bottomClamp = 89.0f;

    // Referencias cacheadas para optimización
    private CharacterController characterController;
    private Transform transformCache;

    // Estado de movimiento
    private Vector3 velocity;
    private float verticalRotation;
    private bool isGrounded;

    // Hash de strings para optimizar el Input Manager viejo si se usa
    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    private void Awake()
    {
        // Cachear componentes para evitar llamadas costosas en Update
        characterController = GetComponent<CharacterController>();
        transformCache = transform;

        // Ocultar y bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        if (cameraTransform == null) return;

        // Lectura de entrada del ratón
        float mouseX = Input.GetAxisRaw(MouseX) * mouseSensitivity;
        float mouseY = Input.GetAxisRaw(MouseY) * mouseSensitivity;

        // Rotación vertical (Pitch)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, topClamp, bottomClamp);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Rotación horizontal del jugador (Yaw)
        transformCache.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;

        // Resetear la velocidad vertical si ya está tocando el suelo
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Pequeña fuerza hacia abajo para mantener el contacto firme
        }

        // Entrada de movimiento
        float inputX = Input.GetAxisRaw(Horizontal);
        float inputZ = Input.GetAxisRaw(Vertical);

        // Selección de velocidad (Caminar / Correr)
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Transformación del vector de movimiento directamente con las direcciones cacheadas
        Vector3 move = (transformCache.right * inputX + transformCache.forward * inputZ).normalized;
        
        characterController.Move(move * (currentSpeed * Time.deltaTime));

        // Salto (Fórmula de física: v = sqrt(h * -2 * g))
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;

        // Aplicar movimiento vertical
        characterController.Move(velocity * Time.deltaTime);
    }
}