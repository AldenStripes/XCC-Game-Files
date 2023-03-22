using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public PlayerHealth script;

    void Start()
    {
        script = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LevelManager.instance.Respawn();
            script.damageTaken = 0;
        }
    }
}
