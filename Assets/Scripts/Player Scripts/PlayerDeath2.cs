using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath2 : MonoBehaviour
{
    public Player2Health script;

    void Start()
    {
        script = GameObject.Find("Player2").GetComponent<Player2Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            LevelManager.instance.RespawnPlayer2();
            script.damageTaken = 0;
        }
    }
}
