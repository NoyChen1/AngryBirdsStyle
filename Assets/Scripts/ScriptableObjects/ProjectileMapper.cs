using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile Mapper")]
public class ProjectileMapper : ScriptableObject
{
    public List<ProjectileEntry> projectiles;

    [System.Serializable]
    public class ProjectileEntry
    {
        public ProjectileType type;
        public ProjectileHandler prefab;
        public int poolSize = 10;
    }
}
