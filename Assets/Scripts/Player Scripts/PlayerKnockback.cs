using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerKnockback : MonoBehaviour //I actually didn't get this script to do what I wanted it to do so I gave up on it but I think it actually helped my knockback system work in a way
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float strength = 16, delay = 0.15f;
    public UnityEvent OnBegin, OnDone;

    public void PlayFeedback(GameObject sender)
    {
        Debug.Log("PlayFeedback works");
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        OnDone?.Invoke();
    }
}
