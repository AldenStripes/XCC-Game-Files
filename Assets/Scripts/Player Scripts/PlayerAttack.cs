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
                // Array stores all enemies found inside the circle hit
                Collider2D[] enemiesFound = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

                for (int i = 0; i < enemiesFound.Length; i++) {
                    enemiesFound[i].GetComponent<Player2Health>().TakeDamage(damage);

                    if (transform.position.x > player2.position.x) {
                        // if player hits player 2 who is to the left
                        player2rb.velocity = new Vector3(-(script.damageTaken), player2rb.velocity.y);
                    } else {
                        // if player hits player 2 who is to the right
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

    // Allows Unity to visualize the attack radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
