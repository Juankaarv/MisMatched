using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperProjectile : MonoBehaviour
{
    public float damage = 10f; // Daño del proyectil
    public float speed = 10f; // Velocidad del proyectil
    private Transform target;

    void Update()
    {
        if (target != null)
        {
            // Moverse hacia el jugador
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Opcional: Puedes hacer que el proyectil gire hacia el jugador
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else
        {
            Destroy(gameObject); // Destruir el proyectil si no hay objetivo
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Destruir el proyectil del boss si colisiona con el proyectil del player
        if (other.CompareTag("PlayerProjectile"))
        {
            Destroy(other.gameObject); // Destruye el proyectil del player
            Destroy(gameObject); // Destruye este proyectil
        }
        // Manejo de colisión con el jugador
        else if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage); // Aplica daño al jugador
                Debug.Log("¡El jugador ha recibido daño del francotirador!"); // Mensaje de depuración
            }
            Destroy(gameObject); // Destruye el proyectil tras la colisión
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget; // Asignar el objetivo (jugador)
    }
}
