using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform respawnPoint;
    public Transform respawnPoint2;
    public Transform checkPoint;
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public AudioSource deathSoundEffect;

    void Start()
    {
        deathSoundEffect = GameObject.FindGameObjectWithTag("DeathSound").GetComponent<AudioSource>();
    }

    private void Awake()
    {
        instance = this;
    }
 
    public void Respawn()
    {
        deathSoundEffect.Play();
        playerPrefab.transform.position = respawnPoint.transform.position;
        Debug.Log("Respawned Player 1");
    }
    public void RespawnPlayer2()
    {
        deathSoundEffect.Play();
        playerPrefab2.transform.position = respawnPoint2.transform.position;
        Debug.Log("Respawned Player 2");
    }
}
