using UnityEngine;
using System;

public class Bullet : MonoBehaviour {
    
    private GameObject target;

    public float speed = 50f;
    public float bulletDamage = 10f;

    //Set the target on the bullet
    public void setBullet (GameObject _target)
    {
        target = _target;
    }

    void Update() 
    {
        //If the bullet doen't have a target, get rid of it
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        //bullet towards to the target.
        Vector3 direction = target.transform.position - transform.position;
        float distancePerFrame = speed * Time.deltaTime;

        //bullet hits the target
        if(direction.magnitude <= distancePerFrame)
        {
            hitTarget();
            return;
        } 

        transform.Translate (direction.normalized * distancePerFrame, Space.World);

    }

     //hitTarget function is called when the bullet hits the target.
     void hitTarget()
    {
        EnemyController targetController = target.GetComponent<EnemyController>();
        targetController.takeDamage(bulletDamage);
        //when bullet hits the target enemy, enemy take damage.
        Debug.Log("Did " + bulletDamage + " damage to " + Enum.GetName(typeof(DataManager.EnemyNames),targetController.enemyDescriptor));
        //destroy the bullet that hits the enemy.
        Destroy(gameObject);
    }
}
