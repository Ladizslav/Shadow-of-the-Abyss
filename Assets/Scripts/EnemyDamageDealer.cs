using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    bool canDealDamage;
    bool hasDealtDamage;

    public BoxCollider weaponCollider;

    [SerializeField] float weaponDamage;

    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = false;

        if (weaponCollider == null)
            weaponCollider = GetComponent<BoxCollider>();

        weaponCollider.enabled = false;
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage = false;
        weaponCollider.enabled = true;
        StartCoroutine(DisableColliderWithDelay());
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
        hasDealtDamage = false;
    }

    IEnumerator DisableColliderWithDelay()
    {
        yield return new WaitForSeconds(0.5f); 
        weaponCollider.enabled = false;
    }


    private void OnDrawGizmos()
    {
        if (weaponCollider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(weaponCollider.center, weaponCollider.size);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasDealtDamage)
            {
                Health playerHealth = other.GetComponent<Health>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(weaponDamage); 
                    hasDealtDamage = true; 
                }
            }
        }
    }

}
