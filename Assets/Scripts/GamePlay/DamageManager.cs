using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;
using System.Threading.Tasks;

public class DamageManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireEffectPrefab;
    [SerializeField] private ParticleSystem explosionEffectPrefab;

    private ObjectPool<ParticleSystem> _firePool;
    private ObjectPool<ParticleSystem> _explosionPool;

    public static DamageManager Instance;
    private DamageManager DamageManagerInstance => DamageManager.Instance;


    private void Awake()
    {
        
        _firePool = new ObjectPool<ParticleSystem>(
            () => Instantiate(fireEffectPrefab),
            actionOnGet: e => e.gameObject.SetActive(true),
            actionOnRelease: e => e.gameObject.SetActive(false),
            actionOnDestroy: e => Destroy(e.gameObject),
            collectionCheck: false,
            defaultCapacity: 10
        );

        _explosionPool = new ObjectPool<ParticleSystem>(
            () => Instantiate(explosionEffectPrefab),
            actionOnGet: e => e.gameObject.SetActive(true),
            actionOnRelease: e => e.gameObject.SetActive(false),
            actionOnDestroy: e => Destroy(e.gameObject),
            collectionCheck: false,
            defaultCapacity: 10
        );

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    public void PlayFireEffect(Vector3 position)
    {
        var effect = _firePool.Get();
        effect.transform.position = position;
        effect.Play();
        _ = ReturnToPool(effect, effect.main.duration, _firePool);
    }

    public void PlayExplosionEffect(Vector3 position)
    {
        var effect = _explosionPool.Get();
        effect.transform.position = position;
        effect.Play();
        _ = ReturnToPool(effect, effect.main.duration, _explosionPool);
    }

    private async Task ReturnToPool(ParticleSystem ps, float delay, ObjectPool<ParticleSystem> pool)
    {
        await Task.Delay((int)(delay * 1000));

        if (ps != null && ps.gameObject.activeInHierarchy)
        {
            pool.Release(ps);
        }
    }
}
