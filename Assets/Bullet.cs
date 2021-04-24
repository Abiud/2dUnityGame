using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float TimeToLive = 5f;
    public int attackDamage = 25;
    public GameObject hitEffect;

     private void Start()
     {
         Destroy(gameObject, TimeToLive);
     }
    void OnCollisionEnter2D(Collision2D collison) {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, .5f);
        if (collison.gameObject.GetComponent<Enemy>() != null)
        {
            collison.gameObject.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
        Destroy(gameObject);
    }
}
