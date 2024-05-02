using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;
    public Animator playerAnim;
    public Rigidbody2D player2rb;
    public Transform player2;
    public Player2Health script;

    void Start()
    {
        script = GameObject.Find("Player2").GetComponent<Player2Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            //then you can attack
            if (Input.GetKey(KeyCode.T))
            {
                playerAnim.SetTrigger("Attack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Player2Health>().TakeDamage(damage);
                    if (transform.position.x > player2.position.x) // If player 1 is to the right of player 2
                    {
                        player2rb.velocity = new Vector3(-(script.damageTaken), player2rb.velocity.y);
                    }
                    else
                    {
                        player2rb.velocity = new Vector3(script.damageTaken, player2rb.velocity.y);
                    }
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
