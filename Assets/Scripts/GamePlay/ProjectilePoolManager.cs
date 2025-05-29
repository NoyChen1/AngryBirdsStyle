using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance { get; private set; }

    private Dictionary<ProjectileType, Queue<ProjectileHandler>> _pools = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public ProjectileHandler GetProjectile(ProjectileType type, Vector3 spawnPosition)
    {
        if (!_pools.ContainsKey(type))
        {
            _pools[type] = new Queue<ProjectileHandler>();
        }

        if (_pools[type].Count > 0)
        {
            var proj = _pools[type].Dequeue();
            proj.transform.position = spawnPosition;
            proj.gameObject.SetActive(true);
            return proj;
        }
        else
        {
            var newProj = Instantiate(type.prefab, spawnPosition, Quaternion.identity)
                            .GetComponent<ProjectileHandler>();
            return newProj;
        }
    }

    public void ReturnProjectile(ProjectileType type, ProjectileHandler projectile)
    {
        projectile.gameObject.SetActive(false);
        _pools[type].Enqueue(projectile);
    }
}
