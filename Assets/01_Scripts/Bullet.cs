using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    float speed = 7f;
    float timeDestroy = 8f;
    public float damage = 1f; // Daño del proyectil
    public string targetTag;  // Tag del objetivo (por ejemplo: "Player" o "Enemy")

    void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el proyectil colisiona con el objetivo correcto
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Si el objetivo es el jugador
            if (targetTag == "Player")
            {
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                }
            }

            // Si el objetivo es un enemigo
            else if (targetTag == "Spider")
            {
                SpiderMan enemy = collision.gameObject.GetComponent<SpiderMan>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            // Destruimos el proyectil después de la colisión
            Destroy(gameObject);
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }



}
