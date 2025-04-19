using UnityEngine;
using TMPro;

public class Healer : MonoBehaviour
{
    public float healAmount = 100f; 
    public TextMeshProUGUI healText; 

    private bool playerInRange = false;
    private Health playerHealth;

    private void Start()
    {
        healText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerHealth = other.GetComponent<Health>();
            healText.gameObject.SetActive(true); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerHealth = null;
            healText.gameObject.SetActive(false); 
        }
    }

    private void Update()
    {
        if (playerInRange && playerHealth != null && Input.GetKeyDown(KeyCode.E))
        {
            HealPlayer();
        }
    }

    private void HealPlayer()
    {
        playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth + healAmount, playerHealth.maxHealth);
        healText.gameObject.SetActive(false); // Skryjeme text po vyléèení
        Destroy(gameObject); // Odstranìní heal objektu po použití
    }
}
