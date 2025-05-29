using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class ProjectileHandler : MonoBehaviour
{
    [SerializeField] private float followDuration = 2.0f;

    private ProjectileType _projectileType;
    private Tween _movementTween;

    private void OnCollisionEnter(Collision collision)
    {
        GameEventDispatcher.DispatchProjectileHit(gameObject, collision.gameObject, _projectileType.damage);
        _movementTween?.Kill();
    }
    public void Launch(List<Vector3> path, float duration, ProjectileType projectileType)
    {
        _projectileType = projectileType;
        gameObject.SetActive(true);
        transform.position = path[0];

        _movementTween = transform.DOPath(
            path.ToArray(),
            duration,
            PathType.CatmullRom
        ).SetEase(Ease.Linear);

        _ = StartLifetimeCountdown(projectileType.lifeTime);
    }

    private async Task StartLifetimeCountdown(float lifetime)
    {
        await Task.Delay((int)(lifetime * 1000));

        if (this == null || !gameObject.activeInHierarchy)
            return;

        Deactivate();
    }

    public void Deactivate()
    {
        _movementTween?.Kill();
        ProjectilePoolManager.Instance.ReturnProjectile(_projectileType, this);
    }

    public void OnHit()
    {
        Deactivate();
    }

}
