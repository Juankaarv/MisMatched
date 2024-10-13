using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float speed = 3f;
    public float health = 50f;
    public GameObject fireProjectilePrefab; // Prefab de proyectil de fuego.
    public GameObject sniperProjectilePrefab; // Prefab de proyectil de francotirador.
    public Transform shootingPoint; // Punto desde donde disparará.
    public float fireProjectileSpeed = 15f; // Velocidad del proyectil de fuego.
    public float sniperProjectileSpeed = 10f; // Velocidad del proyectil de francotirador.

    private Transform player;
    private float attackCooldown = 3f; // Tiempo entre ataques.
    private float attackTimer = 0f;
    private bool isKamikazeActive = false; // Para activar el ataque kamikaze.
    public float detectionRange = 10f; // Rango de detección del jugador.

    private int attackState = 0; // 0 para fuego, 1 para francotirador
    private int fireAttackCount = 0; // Contador para ataques de fuego
    private int sniperAttackCount = 0; // Contador de ataques de francotirador

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        // Verificar si el jugador está dentro del rango de detección
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            MoveTowardsPlayer(); // Sigue al jugador solo si está dentro del rango

            // Si la vida es baja, activa el ataque kamikaze.
            if (health < 40f) // Cambia este valor según sea necesario.
            {
                if (!isKamikazeActive)
                {
                    isKamikazeActive = true;
                }
                KamikazeAttack(); // Ejecuta el ataque kamikaze.
            }
            else
            {
                if (attackTimer >= attackCooldown)
                {
                    ExecuteAttack();
                    attackTimer = 0f; // Reiniciar el temporizador.
                }
            }

        }

    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Calcular la dirección hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;

            // Calcular la rotación que debe tener el boss
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Velocidad de rotación ajustable

            // Mover el boss hacia el jugador en la dirección en la que está mirando
            Vector3 movement = transform.forward * speed * Time.deltaTime;

            // Asegurarse de que el boss se mantenga en el mismo nivel en el eje Y
            movement.y = 0; // Esto asegura que no haya movimiento vertical
            transform.position += movement;

            // Asegurar que el boss no se incline
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = 0; // Fijar el eje X
            eulerRotation.z = 0; // Fijar el eje Z
            transform.eulerAngles = eulerRotation;
        }
    }

    void ExecuteAttack()
    {
        // Intercambiar entre ataques de fuego y francotirador
        if (attackState == 0) // Ataque de fuego
        {
            FireAttack();
            fireAttackCount++;

            // Cambiar al ataque de francotirador después de 2 ataques de fuego
            if (fireAttackCount >= 5)
            {
                attackState = 1; // Cambiar al estado de francotirador
                fireAttackCount = 0; // Reiniciar contador de ataques de fuego
            }
        }
        else if (attackState == 1) // Ataque de francotirador
        {
            SniperAttack();
            sniperAttackCount++;

            // Cambiar de nuevo al ataque de fuego después de 2 zataques de francotirador
            if (sniperAttackCount >= 3)
            {
                attackState = 0; // Cambiar de nuevo al estado de fuego
                sniperAttackCount = 0; // Reiniciar contador de ataques de francotirador
            }
        }
        
    }

    void FireAttack()
    {
        // Dispara dos proyectiles de fuego más rápidos
        for (int i = 0; i < 2; i++)
        {
            // Desplazamiento en el eje X
            Vector3 spawnPosition = shootingPoint.position + new Vector3(i * 0.5f, 0, 0);
            GameObject fireProjectile = Instantiate(fireProjectilePrefab, spawnPosition, shootingPoint.rotation);

            Rigidbody fireRb = fireProjectile.GetComponent<Rigidbody>();
            fireRb.velocity = shootingPoint.forward * fireProjectileSpeed; // Usar la nueva velocidad

            // Agregar un componente para manejar el daño al jugador
            FireProjectile projectileScript = fireProjectile.AddComponent<FireProjectile>();
            projectileScript.damage = 10f; // Asigna daño al proyectil

            Debug.Log("Disparando proyectil " + (i + 1)); // Mensaje de depuración
        }
    }

    void SniperAttack()
    {
        GameObject sniperProjectile = Instantiate(sniperProjectilePrefab, shootingPoint.position, shootingPoint.rotation);
        SniperProjectile sniperProjectileScript = sniperProjectile.GetComponent<SniperProjectile>();
        if (sniperProjectileScript != null)
        {
            sniperProjectileScript.SetTarget(player);
            sniperProjectileScript.damage = 10f; // Asigna daño al proyectil
        }
    }

    void KamikazeAttack()
    {
        // Moverse hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * 2 * Time.deltaTime; // Aumenta la velocidad

        // Verificar colisión con el jugador
        if (Vector3.Distance(transform.position, player.position) < 1f) // Ajusta la distancia según sea necesario
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(20f); // Aplica daño al jugador
            }
            Die(); // Destruir el boss tras el ataque kamikaze
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
