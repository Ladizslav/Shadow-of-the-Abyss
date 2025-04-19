using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    Animator animator;
    Collider[] colliders;
    CharacterController characterController;
    PlayerMovement playerMovement;
    AnimationController animationController;

    private bool isDead = false; 

    public event Action OnDeath;

    private bool isStunned = false;
    private float stunDuration = 0.5f;

    public bool IsStunned() => isStunned;


    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        colliders = GetComponentsInChildren<Collider>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        animationController = GetComponent<AnimationController>();
    }

    public void TakeDamage(float damage)
    {
        if ((gameObject.CompareTag("Player") &&
            (animationController != null && animationController.IsRolling()) || isDead))
        {
            return;
        }

        animator.SetTrigger("damage");
        currentHealth -= damage;

        if (gameObject.CompareTag("Player"))
        {
            StartCoroutine(StunCoroutine());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private IEnumerator StunCoroutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();

        animator.SetTrigger("death");

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (animationController != null)
        {
            animationController.AttackEnd();
        }

        if (gameObject.CompareTag("Player"))
        {
            StartCoroutine(LoadDeathScene());
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }

    private IEnumerator LoadDeathScene()
    {
        yield return new WaitForSeconds(2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Main")
        {
            SceneManager.LoadScene("Death");
        }
        else if (currentScene == "Arena")
        {
            SceneManager.LoadScene("Death 1");
        }
        else
        {
            SceneManager.LoadScene("Death");
        }
    }

    public void SaveHealth()
    {
        PlayerPrefs.SetFloat("PlayerHealth", currentHealth);
        PlayerPrefs.Save();
    }

    public void LoadHealth()
    {
        if (PlayerPrefs.HasKey("PlayerHealth"))
        {
            currentHealth = PlayerPrefs.GetFloat("PlayerHealth");
        }
    }
}