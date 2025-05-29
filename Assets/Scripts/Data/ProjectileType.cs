using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Projectile Type")]
public class ProjectileType : ScriptableObject
{
    public GameObject prefab;
    public float launchForce = 10f;
    public float lifeTime = 10f;
    public float damage = 25f;
    public Material trailMaterial;
}
