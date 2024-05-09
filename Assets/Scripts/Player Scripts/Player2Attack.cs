using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Attack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;
    public Animator playerAnim;
    public Rigidbody2D rb;
    public Transform player1;
    public PlayerHealth script;

    void Start()
    {
        script = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwAttack <= 0)
        {
            //then you can attack
            if (Input.GetKey(KeyCode.U))
            {
                playerAnim.SetTrigger("Attack");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++) {
                    enemiesToDamage[i].GetComponent<PlayerHealth>().TakeDamage(damage);

                    if (transform.position.x > player1.position.x) {
                        // if player hits player 1 who is to the left
                        rb.velocity = new Vector2(-(script.damageTaken), rb.velocity.y);
                    } else {
                        // if player hits player 1 who is to the right
                        rb.velocity = new Vector2(script.damageTaken, rb.velocity.y);
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

    // Allows Unity to visualize the attack radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
