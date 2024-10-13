using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    private Transform playerTarget;     // Objetivo del jugador
    private Transform returnTarget;     // Objetivo de regreso (NedFlanders)
    private float throwSpeed;
    private float returnSpeed;
    private bool isReturning = false;   // Indica si el martillo está en modo de regreso

    public void Initialize(Transform player, Transform returnTarget, float throwSpeed, float returnSpeed)
    {
        this.playerTarget = player;
        this.returnTarget = returnTarget;
        this.throwSpeed = throwSpeed;
        this.returnSpeed = returnSpeed;
    }

    void Update()
    {
        if (isReturning)
        {
            // Retorno del martillo a NedFlanders
            Vector3 direction = (returnTarget.position - transform.position).normalized;
            transform.position += direction * returnSpeed * Time.deltaTime;

            // Destruye el martillo si regresa al enemigo
            if (Vector3.Distance(transform.position, returnTarget.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Lanzamiento del martillo hacia el jugador
            Vector3 direction = (playerTarget.position - transform.position).normalized;
            transform.position += direction * throwSpeed * Time.deltaTime;

            // Verifica si el martillo ha alcanzado al jugador
            if (Vector3.Distance(transform.position, playerTarget.position) < 1f)
            {
                // Aplica daño al jugador
                PlayerController playerController = playerTarget.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(50f); // Daño al jugador
                }
                isReturning = true; // Cambia a modo de regreso
            }
        }
    }
}
