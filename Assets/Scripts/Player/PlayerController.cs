using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("플레이어 설정")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float interactionDistance = 3f;

    private VehicleController currentVehicle;
    private QuarterViewCamera cameraController;
    private CharacterController characterController;
    private Renderer[] playerRenderers;
    private Animator animator;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<QuarterViewCamera>();
        characterController = GetComponent<CharacterController>();
        playerRenderers = GetComponentsInChildren<Renderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleVehicleInteraction();
        HandleMovement();
    }

    private void HandleVehicleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentVehicle == null)
            {
                TryEnterVehicle();
            }
            else
            {
                ExitVehicle();
            }
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (currentVehicle != null)
        {
            currentVehicle.ControlVehicle(horizontal, vertical);
        }
        else
        {
            MovePlayer(horizontal, vertical);
        }
    }

    private void MovePlayer(float horizontal, float vertical)
    {
        if (characterController != null && characterController.enabled)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 movement = (forward * vertical + right * horizontal).normalized;

            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                characterController.Move(movement * moveSpeed * Time.deltaTime);
                animator.SetBool("isWalk", true);
            }
            else
            {
                animator.SetBool("isWalk", false);
            }
        }
    }

    private void TryEnterVehicle()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        foreach (Collider collider in colliders)
        {
            VehicleController vehicle = collider.GetComponent<VehicleController>();
            if (vehicle != null)
            {
                currentVehicle = vehicle;
                currentVehicle.EnterVehicle(this);
                cameraController.target = currentVehicle.transform;
                characterController.enabled = false;
                SetPlayerVisibility(false);
                return;
            }
        }
    }


    private void ExitVehicle()
    {
        if (currentVehicle != null)
        {
            Vector3 exitPosition = currentVehicle.transform.position - currentVehicle.transform.right * 1.5f;
            transform.position = exitPosition;

            currentVehicle.ExitVehicle(this);
            cameraController.target = this.transform;
            characterController.enabled = true;
            SetPlayerVisibility(true);
            currentVehicle = null;
        }
    }

    public void SetPlayerVisibility(bool isVisible)
    {
        foreach (Renderer renderer in playerRenderers)
        {
            if (renderer != null)
            {
                renderer.enabled = isVisible;
            }
        }
    }
}