using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damageAmount = 100; // Množství poškození, které zpùsobí útok

    // Metoda pro zpracování kolizí s nepøáteli
    private void OnTriggerEnter(Collider other)
    {
        // Kontrola, zda kolizní objekt má tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Získání reference na komponentu Health nepøátelského objektu, pokud existuje
            Health enemyHealth = other.GetComponent<Health>();

            // Pokud se podaøilo získat referenci na komponentu Health, zraníme nepøítele
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount); // Zavolání metody TakeDamage s množstvím poškození
            }
            else
            {
                Debug.LogWarning("Nepøátelský objekt neobsahuje komponentu Health."); // Upozornìní, pokud nepøátelský objekt neobsahuje komponentu Health
            }
        }
    }
}
