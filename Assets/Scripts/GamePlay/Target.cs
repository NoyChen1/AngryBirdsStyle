using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float delayBeforeDestroy = 0f;
    [SerializeField] private float initialHealth = 100f;

    [SerializeField] private float health;

    private DamageManager DamageManager => DamageManager.Instance;

    private void OnEnable()
    {
        health = initialHealth;
        GameEventDispatcher.OnProjectileHit += HandleProjectileHit;
    }

    private void OnDisable()
    {
        GameEventDispatcher.OnProjectileHit -= HandleProjectileHit;
    }

    private void HandleProjectileHit(GameObject projectile, GameObject target, float damage)
    {
        if (target == gameObject)
        {
            DecreaseLife(damage, projectile);
            Hit();
        }
    }

    private void DecreaseLife(float damage, GameObject projectile)
    {
        if(health <= 0)
        {
            return;
        }
        else
        {
            /*
            if (damageManager == null) {
                Debug.Log("damage manager null");
                return;
            }*/

            DamageManager?.PlayFireEffect(projectile.transform.position);
            health -= damage;
        }
    }
    public void Hit()
    {
        Debug.Log($"{gameObject.name} Hitted!");

        /*
        if (destroyEffect != null)
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

        */

        if(health <= 0f)
        {
            DamageManager?.PlayExplosionEffect(transform.position);
            Deactivate();
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        TargetManager.Instance?.CheckIfAllTargetsDestroyed();
    }
}
