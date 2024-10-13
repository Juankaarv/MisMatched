using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject projectilePrefab; // Aquí se asignará el prefab en el inspector.
    public float projectileSpeed = 10f;
    public Transform shootingPoint; // Referencia al objeto vacío que actuará como punto de disparo.
    public float health = 100f; // Vida del jugador

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
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
        // Lógica para la muerte del jugador (ejemplo: desactivar el jugador, mostrar pantalla de muerte, etc.)
        gameObject.SetActive(false); // Desactivar el jugador
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);
    }

    void Shoot()
    {
        // Instanciamos el proyectil en la posición del ShootingPoint.
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.velocity = shootingPoint.forward * projectileSpeed;
    }
}
