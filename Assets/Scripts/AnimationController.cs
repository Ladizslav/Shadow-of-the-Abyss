using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private float lastClickedTime = 0;
    private const float maxComboDelay = 1;
    private const float COOLDOWN_TIME = 2f;
    private float nextFireTime = 0f;
    private int noOfClicks = 0;
    private bool isRunning;
    private bool isRolling;
    private PauseManager pauseManager;

    [SerializeField] private BoxCollider colliderWeapon;
    [SerializeField] private CapsuleCollider capsuleCollider;

    private string[] hitAnimations = { "hit1", "hit2", "hit3", "hit4", "hit5" };

    void Start()
    {
        animator = GetComponent<Animator>();
        colliderWeapon.enabled = false;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (!PauseManager.isPaused)
        {
            // Reset hit animations if in idle state
            if (IsInIdleState())
            {
                ResetAllHitAnimations();
            }

            CheckHitAnimationCompletion();

            if (Time.time - lastClickedTime > maxComboDelay)
            {
                noOfClicks = 0;
            }

            if (Time.time > nextFireTime)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnClick();
                }
            }

            capsuleCollider.enabled = !isRolling;
        }
    }

    // Check if character is in idle state
    private bool IsInIdleState()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f;
    }

    // Reset all hit animation bools to false
    private void ResetAllHitAnimations()
    {
        foreach (var hitAnimation in hitAnimations)
        {
            animator.SetBool(hitAnimation, false);
        }
        noOfClicks = 0;
    }

    // Check if hit animations are nearly complete
    private void CheckHitAnimationCompletion()
    {
        foreach (var hitAnimation in hitAnimations)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f &&
                animator.GetCurrentAnimatorStateInfo(0).IsName(hitAnimation))
            {
                animator.SetBool(hitAnimation, false);
                if (hitAnimation == "hit5")
                {
                    noOfClicks = 0;
                }
            }
        }
    }

    public void SetMovementSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void SetRunning(bool isRunning)
    {
        this.isRunning = isRunning;
        animator.SetBool("isRunning", isRunning);
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void Roll()
    {
        isRolling = true;
        animator.SetBool("isRolling", true);
        StartCoroutine(EndRollAnimation());
    }

    private IEnumerator EndRollAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isRolling", false);
        isRolling = false;
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        noOfClicks = Mathf.Clamp(noOfClicks, 0, hitAnimations.Length);

        if (noOfClicks >= 1)
        {
            animator.SetBool(hitAnimations[noOfClicks - 1], true);
        }
    }

    public void AttackStart()
    {
        colliderWeapon.enabled = true;
    }

    public void AttackEnd()
    {
        colliderWeapon.enabled = false;
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    public bool IsInHitAnimation()
    {
        foreach (var hitAnimation in hitAnimations)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(hitAnimation))
            {
                return stateInfo.normalizedTime < 0.6f;
            }
        }
        return false;
    }
}