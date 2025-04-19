using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float attackCD = 3f;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float aggroRange = 4f;

    protected GameObject player;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected float timePassed;
    protected float newDestinationCD = 0.5f;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed); // aktualizace rychlosti animace

        if (player == null) return;

        Vector3 playerPositionWithoutY = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z); // pozice hr��e bez Y sou�adnice
        transform.LookAt(playerPositionWithoutY); // nato�it se sm�rem k hr��i

        if (timePassed >= attackCD)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= attackRange) // pokud je hr�� v dosahu �toku
            {
                animator.SetTrigger("attack");
                timePassed = 0;
            }
        }
        timePassed += Time.deltaTime;

        if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange) // pokud je cooldown pro nov� c�l hotov� a hr�� je v dosahu agrese
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position);
        }
        newDestinationCD -= Time.deltaTime;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    public virtual void StartDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
    }

    public virtual void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

    protected virtual void OnDestroy()
    {
        // Z�sk�me odkaz na SpawnerManager, abychom mohli zavolat spr�vnou funkci
        SpawnerManager spawnerManager = GetComponentInParent<SpawnerManager>();

        if (spawnerManager != null)
        {
            // Zavol�me metodu pro odstran�n� nep��tele
            spawnerManager.EnemyDestroyed(gameObject);
        }
    }




}
