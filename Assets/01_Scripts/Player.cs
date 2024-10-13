using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float moveSpeed = 3f;
    public float projectileSpeed = 1f;
    public int lives = 7;
    public float speedIncreaseAmount = 0.5f;
    public float projectileSpeedIncreaseAmount = 0.5f;

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
            lives--;

            // Destruye la bala enemiga
            Destroy(other.gameObject);

            // Comprueba si las vidas del jugador llegan a 0
            if (lives <= 0)
            {
                // Aquí puedes poner la lógica para cuando el jugador pierda todas sus vidas (como reiniciar el nivel o mostrar un mensaje de game over)
                Debug.Log("Game Over");
                Destroy(gameObject);
            }
        }
    }
}
