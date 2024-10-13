using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMan : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab del proyectil que disparar� el enemigo
    public Transform firePoint; // Punto desde donde el enemigo dispara
    public float projectileSpeed = 5f; // Velocidad de las balas
    public float shootInterval = 3f; // Intervalo entre disparos
    public float attackRange = 2f; // Rango para atacar cuerpo a cuerpo
    public float life = 5f; // Vida del enemigo
    public float damage = 1f; // Da�o que hace el enemigo al atacar
    public float moveSpeed = 3f; // Velocidad de movimiento del enemigo
    public float chaseSpeed = 5f; // Velocidad al perseguir al jugador

    private GameObject player; // Referencia al jugador
    private bool canShoot = true; // Controla si puede disparar
    private Animator animator; // Referencia al animador

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Debug.LogError("No se encontr� al jugador. Aseg�rate de que el jugador tenga la etiqueta 'Player'.");
        }

        // Comienza a disparar en intervalos regulares
        InvokeRepeating("TryShoot", shootInterval, shootInterval);
    }

    void Update()
    {
        if (player == null) return;

        // El enemigo siempre persigue al jugador
        ChasePlayer();

        // Si el jugador est� en rango de ataque cuerpo a cuerpo, ataca
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
    }

    // Intenta disparar si no est� atacando cuerpo a cuerpo
    void TryShoot()
    {
        if (canShoot)
        {
            Shoot();
        }
    }

    // M�todo para disparar balas hacia el jugador
    void Shoot()
    {
        if (player == null) return;

        // Crea el proyectil en el punto de disparo y asigna la rotaci�n hacia adelante
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Calcula la direcci�n hacia el jugador
            Vector3 directionToPlayer = (player.transform.position - firePoint.position).normalized;
            rb.velocity = directionToPlayer * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("El proyectil no tiene un componente Rigidbody.");
        }
    }

    // M�todo para perseguir al jugador
    void ChasePlayer()
    {
        if (player == null) return;

        // Calcula la direcci�n hacia el jugador
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Mant�n la rotaci�n solo en el eje Y

        // Rota hacia el jugador gradualmente
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);

        // Mueve al enemigo hacia el jugador
        transform.position += direction * chaseSpeed * Time.deltaTime;

        // Activa la animaci�n de persecuci�n si existe
        if (animator != null)
        {
            animator.SetBool("isRunning", true); // Asume que tienes una animaci�n de correr
        }
    }

    // M�todo para atacar al jugador cuerpo a cuerpo
    void AttackPlayer()
    {
        if (player == null) return;

        Player playerLogic = player.GetComponent<Player>();
        if (playerLogic != null)
        {
            playerLogic.TakeDamage(damage); // Inflige da�o al jugador

            // Activar animaci�n de ataque si existe
            if (animator != null)
            {
                animator.SetTrigger("attack"); // Suponiendo que tienes un trigger de ataque en el Animator
            }
        }
    }

    // M�todo para recibir da�o
    public void TakeDamage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Die(); // Destruye al enemigo si su vida llega a 0
        }
    }

    void Die()
    {
        // Efectos de muerte como animaciones o sonidos
        Destroy(gameObject); // Destruye el objeto del enemigo
    }

    // M�todo de colisi�n para hacer da�o al jugador
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player playerLogic = collision.gameObject.GetComponent<Player>();
            if (playerLogic != null)
            {
                playerLogic.TakeDamage(2f); // Inflige da�o al jugador al colisionar
            }
        }
    }



    // M�todo para hacer da�o al jugador cuando colisiona cuerpo a cuerpo
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerLogic = other.GetComponent<Player>();
            if (playerLogic != null)
            {
                playerLogic.TakeDamage(damage); // Inflige da�o al jugador
                Debug.Log("El enemigo ha golpeado al jugador");
            }
        }
    }



}
