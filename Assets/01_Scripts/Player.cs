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
    public float speedIncreaseAmount = 0.5f;
    public float projectileSpeedIncreaseAmount = 0.5f;

    public Image barraDeVida; // La barra de vida en la UI
    private int currentLives;

    void Start()
    {
        // Inicializa las vidas del jugador con el valor máximo
        currentLives = maxLives;
        UpdateHealthBar();
    }

    void Update()
    {
        Move();

        if (Input.GetButtonDown("Fire1")) // Disparo con el botón configurado como "Fire1" (por defecto clic izquierdo)
        {
            Shoot();
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

    void Shoot()
    {
        // Crea el proyectil en la posición del punto de disparo y con la misma rotación
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Asigna velocidad al proyectil
        rb.velocity = firePoint.forward * projectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que colisiona es un objeto recolectable que aumenta la velocidad de movimiento
        if (other.gameObject.CompareTag("SpeedBoost"))
        {
            moveSpeed += speedIncreaseAmount;
            Destroy(other.gameObject);
        }

        // Comprueba si el objeto que colisiona es un objeto recolectable que aumenta la velocidad de disparo
        if (other.gameObject.CompareTag("FireRateBoost"))
        {
            projectileSpeed += projectileSpeedIncreaseAmount;
            Destroy(other.gameObject);
        }

        // Comprueba si el objeto que colisiona es una bala enemiga
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            // Reduce las vidas del jugador y actualiza la barra de vida
            currentLives--;
            UpdateHealthBar();

            // Destruye la bala enemiga
            Destroy(other.gameObject);

            // Comprueba si las vidas del jugador llegan a 0
            if (currentLives <= 0)
            {
                // Aquí puedes poner la lógica para cuando el jugador pierda todas sus vidas (como reiniciar el nivel o mostrar un mensaje de game over)
                Debug.Log("Game Over");
                Destroy(gameObject);
            }
        }
    }

    void UpdateHealthBar()
    {
        // Actualiza la barra de vida en la UI
        if (barraDeVida != null)
        {
            barraDeVida.fillAmount = (float)currentLives / maxLives;
        }
    }
}
