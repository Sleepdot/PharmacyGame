using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public DataManager.EnemyNames enemyDescriptor;
    public float enemySpeed = 5.0f;

    public int enemyDamage = 5;

    public float enemyHealth = 20;

    public Transform[] waypoints;

    public int currentWaypoint;

    public int reward = 5; //The amount you get from killing this enemy
    public AudioClip enemyDamageSound; // sound effect for enemy doing damage to player
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentWaypoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //Move() follows the path of waypoints
    void Move(){
        if(currentWaypoint < waypoints.Length){
            //Move towards the next waypoint
            transform.position = Vector3.MoveTowards(transform.position, 
                                                    waypoints[currentWaypoint].position, 
                                                    Time.deltaTime * enemySpeed);
            //Update the waypoint once the current one is reached
            if(transform.position == waypoints[currentWaypoint].position){
                currentWaypoint += 1;
            }
            //If the final waypoint is reached
            if(currentWaypoint == waypoints.Length){
                GameManager.playerHealth -= enemyDamage;
                Debug.Log(GameManager.playerHealth);
                StartCoroutine(PlayDamageNoise());
            }
        }
    }


    public void takeDamage (float damage)
    {
        enemyHealth -= damage;

        if(enemyHealth <= 0)
        {
            Die();
        }
    }

        void Die()
    {
        GameManager.currency += reward;
        Destroy(gameObject);
    }

    IEnumerator PlayDamageNoise(){
        audioSource.PlayOneShot(enemyDamageSound, 1.0f);
        yield return new WaitForSeconds(0.75f);
        Destroy(gameObject);
    }
}


