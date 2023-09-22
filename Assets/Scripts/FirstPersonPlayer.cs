using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonPlayer : MonoBehaviour
{
    SaveManager savePlayer;
    PlayerInfo play;

    [Header("General")]
    [SerializeField] private Object enemy;
    [SerializeField] private float movementSpeed = 15f;
    [SerializeField] private TextMeshProUGUI pause;
    [SerializeField] private Button resume;
    [SerializeField] private Button saveExit;
    [SerializeField] private Image panel;
    private bool isPaused = false;

    [Header("Falling")]
    [SerializeField] private float gravityFactor = 1f;
    [SerializeField] private Transform groundPosition;
    [SerializeField] private LayerMask groundLayers;

    [Header("Jumping")]
    [SerializeField] private bool canAirControl = true;
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private bool canInteract = true;

    [Header("Looking")]
    [SerializeField] private float mouseSensitivity = 1000f;
    [SerializeField] private Camera camera;
    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;
    private CharacterController controller;
    private float verticalRotation = 0f;
    private float verticalSpeed = 0f;
    private bool isGrounded = false;

    [Header("Potions")]
    [SerializeField] Image greenPot;
    [SerializeField] Image pinkPot;
    [SerializeField] Image yellowPot;
    private bool gr;
    private bool pi;
    private bool ylw;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        savePlayer = new SaveManager();
        play = new PlayerInfo();
        isPaused = false;
        pause.gameObject.SetActive(false);
        resume.gameObject.SetActive(false);
        saveExit.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        greenPot.gameObject.SetActive(false);
        pinkPot.gameObject.SetActive(false);
        yellowPot.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        verticalRotation = 0f;

        if (MainMenu.isLoad == true)
        {
            savePlayer.LoadArmour();
            this.gameObject.transform.position = savePlayer.player.position;
            this.gr = savePlayer.player.green;
            this.pi = savePlayer.player.pink;
            this.ylw = savePlayer.player.yellow;

            if (this.gr==true)
            {
                greenPot.gameObject.SetActive(true);
            }

            if (this.pi == true)
            {
                pinkPot.gameObject.SetActive(true);
            }

            if (this.ylw == true)
            {
                yellowPot.gameObject.SetActive(true);
            }
        }

        else
        {
            savingPlayer();
        }
    }

    private void savingPlayer()
    {
        this.gr = false;
        this.pi = false;
        this.ylw = false;
        play.position = this.gameObject.transform.position;

        if (greenPot.gameObject.activeInHierarchy == true)
        {
            this.gr = true;
        }

        if (pinkPot.gameObject.activeInHierarchy == true)
        {
            this.pi = true;
        }

        if (yellowPot.gameObject.activeInHierarchy == true)
        {
            this.ylw = true;
        }

        play.green = this.gr;
        play.pink = this.pi;
        play.yellow = this.ylw;
        savePlayer.SavePlayer(play);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                savingPlayer();
                pause.gameObject.SetActive(true);
                resume.gameObject.SetActive(true);
                saveExit.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }

            else
            {
                Time.timeScale = 1;
            }

        }

        else
        {
            isPaused = false;
        }

        // are we on the ground?
        RaycastHit collision;
        if (Physics.Raycast(groundPosition.position, Vector3.down, out collision, 0.2f, groundLayers))
        {
            isGrounded = true;
        }

        else
        {
            isGrounded = false;
        }

        // update vertical speed
        if (!isGrounded)
        {
            verticalSpeed += gravityFactor * -9.81f * Time.deltaTime;
        }

        if (canInteract)
        {
            HandleInteractionCheck();
            HandleInteractionInput();
        }

        else
        {
            verticalSpeed = 0f;
        }

        // adjust rotations based on mouse position
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        camera.transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);

        Vector3 x = Vector3.zero;
        Vector3 y = Vector3.zero;
        Vector3 z = Vector3.zero;

        // handle jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            verticalSpeed = jumpSpeed;
            isGrounded = false;
            y = transform.up * verticalSpeed;
        }

        else if (!isGrounded)
        {
            y = transform.up * verticalSpeed;
        }

        // handle movement
        if (isGrounded || canAirControl)
        {
            x = transform.right * Input.GetAxis("Horizontal") * movementSpeed;
            z = transform.forward * Input.GetAxis("Vertical") * movementSpeed;
        }

        Vector3 movement = x + y + z;
        movement *= Time.deltaTime;
        controller.Move(movement);
    }

    private void HandleInteractionCheck()
    {
        if (Physics.Raycast(camera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer==9 && (currentInter==null || hit.collider.gameObject.GetInstanceID() != currentInter.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInter);

                if (currentInter)
                {
                    currentInter.OnFocus();
                }
            }
        }

        else if (currentInter)
        {
            currentInter.OnLoseFocus();
            currentInter = null;
        }
    }
    
    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(interactKey) && currentInter!=null && Physics.Raycast(camera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInter.OnInteract();
        }

    }

    public void Resume()
    {
        Time.timeScale = 1;
        pause.gameObject.SetActive(false);
        resume.gameObject.SetActive(false);
        saveExit.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Witch"))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOver");
        }
    }

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInter;

}