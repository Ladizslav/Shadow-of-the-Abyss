using UnityEngine;

public class Damager : MonoBehaviour
{
    public int damageAmount = 100; // Mno�stv� po�kozen�, kter� zp�sob� �tok

    // Metoda pro zpracov�n� koliz� s nep��teli
    private void OnTriggerEnter(Collider other)
    {
        // Kontrola, zda kolizn� objekt m� tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Z�sk�n� reference na komponentu Health nep��telsk�ho objektu, pokud existuje
            Health enemyHealth = other.GetComponent<Health>();

            // Pokud se poda�ilo z�skat referenci na komponentu Health, zran�me nep��tele
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount); // Zavol�n� metody TakeDamage s mno�stv�m po�kozen�
            }
            else
            {
                Debug.LogWarning("Nep��telsk� objekt neobsahuje komponentu Health."); // Upozorn�n�, pokud nep��telsk� objekt neobsahuje komponentu Health
            }
        }
    }
}
