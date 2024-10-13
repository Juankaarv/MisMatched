using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float damage = 10f; // Da�o del proyectil
    public float speed = 10f; // Velocidad del proyectil

    void Update()
    {
        // Mover el proyectil hacia adelante
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        

        // Verifica si colisiona con el boss
        if (other.CompareTag("Boss"))
        {
            BossController bossController = other.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.TakeDamage(damage); // Aplica da�o al boss
            }
            Destroy(gameObject); // Destruir este proyectil despu�s de da�ar al boss
        }
        else if (other.CompareTag("NedFlanders"))
        {
            NedFlandersController nedController = other.GetComponent<NedFlandersController>();
            if (nedController != null)
            {
                nedController.TakeDamage(damage); // Aplica da�o a Ned Flanders
            }
            Destroy(gameObject); // Destruir este proyectil despu�s de da�ar a Ned Flanders
        }
    }
}
