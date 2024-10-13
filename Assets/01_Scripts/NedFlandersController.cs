using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NedFlandersController : MonoBehaviour
{
    public GameObject hammerPrefab;      // Prefab del martillo
    public Transform throwingPoint;      // Punto desde donde lanza el martillo
    public float throwSpeed = 10f;       // Velocidad de lanzamiento
    public float returnSpeed = 5f;       // Velocidad de retorno
    public float attackCooldown = 8f;    // Tiempo entre ataques
    private float lastAttackTime = 0f;   // Para controlar el cooldown
    private Transform player;
    public float health = 50f;

    public float detectionDistance = 15f; // Distancia para detectar al jugador
    public float followDistance = 5f;     // Distancia mínima a la que se mantendrá de distancia del jugador
    public float followSpeed = 3f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Verifica si el jugador está dentro de la distancia de detección
        if (Vector3.Distance(transform.position, player.position) <= detectionDistance)
        {
            // Calcula la dirección hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;

            // Calcula la distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Verifica si está dentro de la distancia mínima de seguimiento
            if (distanceToPlayer > followDistance)
            {
                // Mueve a Ned Flanders hacia el jugador
                transform.position += direction * followSpeed * Time.deltaTime;
            }

            // Ataca si está en rango y el cooldown ha pasado
            if (Time.time > lastAttackTime + attackCooldown && distanceToPlayer <= 15f)
            {
                ThrowHammer();
                lastAttackTime = Time.time;  // Actualiza el tiempo del último ataque
            }
        }
    }

    void ThrowHammer()
    {
        // Instancia el martillo
        GameObject hammer = Instantiate(hammerPrefab, throwingPoint.position, throwingPoint.rotation);
        HammerController hammerController = hammer.GetComponent<HammerController>();
        if (hammerController != null)
        {
            hammerController.Initialize(player, transform, throwSpeed, returnSpeed); // Asigna el jugador como objetivo y la posición de retorno
        }
    }
    public void TakeDamage(float damage)
    {
        // Lógica para recibir daño (puedes agregar lógica para morir si su salud llega a cero)
        // Por ejemplo, si tienes un campo de salud:
        health -= damage; // Suponiendo que hay un campo de salud en Ned Flanders
        if (health <= 0)
        {
            Die(); // Método para manejar la muerte
        }
    }

    void Die()
    {
        // Lógica para la muerte de Ned Flanders (ejemplo: desactivar el enemigo, mostrar efectos, etc.)
        gameObject.SetActive(false); // Desactivar Ned Flanders
    }
}
