using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float sprintSpeed = 4f;
    public float rollSpeed = 6f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f; // Pøidána gravitace

    private CharacterController characterController;
    private AnimationController animationController;
    private Vector3 moveDirection;
    private Vector3 velocity; // Rychlost pádu
    private bool isRolling = false;

    private PauseManager pauseManager;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!PauseManager.isPaused)
        {
            bool isAttacking = animationController.IsInHitAnimation();
            bool isStunned = GetComponent<Health>().IsStunned();
            bool canMove = !isAttacking && !isRolling && !isStunned;

            // Vstupy - pohyb povolen pouze když canMove je true
            float horizontalInput = canMove ? Input.GetAxis("Horizontal") : 0f;
            float verticalInput = canMove ? Input.GetAxis("Vertical") : 0f;

            // smerovy vektor pohybu relativni k pohledu kamery
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward = camForward.normalized;
            camRight = camRight.normalized;

            // vypocet smeru pohybu relativne k pohledu kamery
            moveDirection = camForward * verticalInput + camRight * horizontalInput;

            // normalizace smeru pohybu
            if (moveDirection.magnitude > 1f)
                moveDirection.Normalize();

            // rotace postavy
            if (moveDirection != Vector3.zero && !isRolling)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // animace chuze
            float moveSpeed = walkSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
                animationController.SetRunning(true); // spusti animaci behu
            }
            else
            {
                animationController.SetRunning(false); // zastavi animaci behu
            }

            // kontrola pro spusteni roll animace
            if (!isRolling && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(RollCoroutine());
            }

            // kontrola, zda je hráè na zemi
            if (characterController.isGrounded)
            {
                if (velocity.y < 0)
                {
                    velocity.y = -2f; // reset rychlosti pøi kontaktu se zemí
                }
            }

            // aplikace gravitace
            velocity.y += gravity * Time.deltaTime;

            // pohyb postavy
            if (!isRolling)
            {
                characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            }

            characterController.Move(velocity * Time.deltaTime); // aplikace gravitace na pohyb

            // aktualizace rychlosti pohybu pro animaci
            animationController.SetMovementSpeed(moveDirection.magnitude * moveSpeed);
        }
    }

    IEnumerator RollCoroutine()
    {
        // zacatek roll animace
        isRolling = true;
        animationController.Roll(); // spusti roll animaci

        // zrychleni postavy ve smeru rollu
        Vector3 rollDirection = transform.forward;
        float rollSpeed = 10f;
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            characterController.Move(rollDirection * rollSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // pockani na dokonceni animace
        yield return new WaitForSeconds(0.5f);

        // konec roll animace
        isRolling = false;
    }

    public void SavePosition()
    {
        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);
        PlayerPrefs.Save();
    }

    public void LoadPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            transform.position = new Vector3(x, y, z);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy")) 
        {
            Vector3 pushDirection = (transform.position - hit.transform.position).normalized;
            characterController.Move(pushDirection * 0.2f);
        }
    }
}