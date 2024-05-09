using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int damageTaken;
    public GameObject hitEffect;
    public Animator camAnim;
    [SerializeField] private AudioSource hitSoundEffect;
    public UnityEvent OnHit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        OnHit?.Invoke();
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        camAnim.SetTrigger("Shake");
        hitSoundEffect = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        hitSoundEffect.Play();
        damageTaken += damage;
        if (damageTaken > 30) //Cap at 30 damage taken
        {
            damageTaken = 30;
        }
        Debug.Log("Player 1 Damage Taken");
    }
}
