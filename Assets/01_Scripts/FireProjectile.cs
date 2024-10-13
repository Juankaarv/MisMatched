using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float damage = 10f; // Daño del proyectil
    public float speed = 10f; // Velocidad del proyectil

    void Update()
    {
        // Mover el proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si colisiona con el proyectil del jugador
        if (other.CompareTag("PlayerProjectile"))
        {
            Destroy(other.gameObject); // Destruir el proyectil del jugador
            Destroy(gameObject); // Destruir este proyectil
        }

        // Verifica si colisiona con el jugador
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage); // Aplica daño al jugador
            }
            Destroy(gameObject); // Destruir el proyectil después de dañar al jugador
        }
    }
}
