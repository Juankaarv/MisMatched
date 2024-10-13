using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float moveSpeed = 3f;
    public float projectileSpeed = 1f;
    public int maxLives = 7;
    public Image barraDeVida; // La barra de vida en la UI
    private float currentLives;
    private bool isDead = false; // Variable para controlar si el jugador ha muerto

    void Start()
    {
        // Inicializa las vidas del jugador con el valor máximo
        currentLives = maxLives;
        UpdateHealthBar();
    }

    void Update()
    {
        // Verifica si el jugador está vivo antes de mover o disparar
        if (!isDead)
        {
            Move();
            HandleShooting(); // Separamos el disparo en otro método para mayor claridad
        }
    }

    void Move()
    {
        // Obtiene la entrada del jugador
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calcula la dirección en base a la dirección actual del jugador
        Vector3 moveDirection = (transform.forward * moveZ) + (transform.right * moveX);

        // Mueve el jugador
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleShooting()
    {
        // Disparo con la tecla Espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Verifica si el proyectil no ha sido destruido y firePoint existe
        if (projectilePrefab != null && firePoint != null)
        {
            // Crea el proyectil en la posición del punto de disparo y con la misma rotación
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            // Comprueba si el proyectil tiene un Rigidbody asignado y asigna la velocidad
            if (rb != null)
            {
                rb.velocity = firePoint.forward * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("El proyectil no tiene un componente Rigidbody.");
            }
        }
        else
        {
            Debug.LogWarning("El prefab del proyectil o firePoint no están asignados.");
        }
    }

    // Método para recibir daño
    public void TakeDamage(float damage)
    {
        currentLives -= damage;
        UpdateHealthBar();

        // Comprueba si las vidas del jugador llegan a 0
        if (currentLives <= 0 && !isDead)
        {
            Debug.Log("Game Over");
            isDead = true; // Marca al jugador como muerto
            Destroy(gameObject); // Destruye al jugador cuando se queda sin vidas
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que colisiona es una bala enemiga
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            TakeDamage(1); // Recibe daño por bala enemiga
            Destroy(other.gameObject); // Destruye la bala enemiga
        }

        // Comprueba si el objeto que colisiona es un enemigo para recibir daño cuerpo a cuerpo
        if (other.gameObject.CompareTag("Spider"))
        {
            TakeDamage(2); // Recibe daño cuerpo a cuerpo por el enemigo
            Debug.Log("El jugador ha sido golpeado por el enemigo");
        }
    }


    // Método para actualizar la barra de vida
    void UpdateHealthBar()
    {
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = (float)currentLives / maxLives;
        }
    }


}
