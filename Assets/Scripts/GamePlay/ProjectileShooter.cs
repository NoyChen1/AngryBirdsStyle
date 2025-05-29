using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Ammo")]
    [SerializeField] private List<ProjectileType> projectileTypes;
    [SerializeField] private int defaultAmmoIndex = 0;
    [SerializeField] private int ammoPerType = 5;

    [Header("Shooting")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float duration = 3f;

    private List<Vector3> _lastTrajectory;
    private int _currentIndex;

    private Dictionary<ProjectileType, int> _ammoByType = new();


    private void OnEnable()
    {
        GameEventDispatcher.OnTrajectoryUpdated += UpdateTrajectory;
        GameEventDispatcher.OnShoot += LaunchProjectile;
        GameEventDispatcher.OnAmmoSwitchRequested += SwitchAmmo;

        InitAmmo();
        DispatchCurrentAmmo();
    }

    private void OnDisable()
    {
        GameEventDispatcher.OnTrajectoryUpdated -= UpdateTrajectory;
        GameEventDispatcher.OnShoot -= LaunchProjectile;
        GameEventDispatcher.OnAmmoSwitchRequested -= SwitchAmmo;

    }

    private void InitAmmo()
    {
        foreach (var type in projectileTypes)
        {
            _ammoByType[type] = ammoPerType;
        }
        _currentIndex = Mathf.Clamp(defaultAmmoIndex, 0, projectileTypes.Count - 1);
    }

    private void SwitchAmmo()
    {
        int originalIndex = _currentIndex;

        do
        {
            _currentIndex = (_currentIndex + 1) % projectileTypes.Count;

            if (_ammoByType[projectileTypes[_currentIndex]] > 0)
                break;

        } while (_currentIndex != originalIndex); 

        DispatchCurrentAmmo();
    }



    private void LaunchProjectile(Vector3 direction)
    {
        if (_lastTrajectory == null || _lastTrajectory.Count < 2) return;

        ProjectileType currentType = projectileTypes[_currentIndex];

        if (_ammoByType[currentType] <= 0)
        {
            Debug.LogWarning($"No Ammo of type {currentType}");
            return;
        }

        _ammoByType[currentType]--;
        DispatchCurrentAmmo();

        _ = DelayedCheckAmmoAndTargets();

        ProjectileHandler projectile = ProjectilePoolManager.Instance.GetProjectile(currentType, spawnPoint.position);

        //ProjectileHandler projectile = Instantiate(currentType.prefab, spawnPoint.position, Quaternion.identity).
          //  GetComponent<ProjectileHandler>();
        projectile.Launch(_lastTrajectory, duration: 3f, currentType);
    }

    private void DispatchCurrentAmmo()
    {
        var type = projectileTypes[_currentIndex];
        GameEventDispatcher.DispatchAmmoChanged(type, _ammoByType[type]);
    }

    private async System.Threading.Tasks.Task DelayedCheckAmmoAndTargets()
    {
        await System.Threading.Tasks.Task.Delay(3000); 

        CheckAmmoAndTargets();
    }

    private void UpdateTrajectory(List<Vector3> trajectory)
    {
        _lastTrajectory = new List<Vector3>(trajectory);
    }

    private void CheckAmmoAndTargets()
    {
        bool hasAnyAmmo = false;

        foreach (var type in projectileTypes)
        {
            if (_ammoByType[type] > 0)
            {
                hasAnyAmmo = true;
                break;
            }
        }

        if (!hasAnyAmmo && TargetManager.Instance.HasActiveTargets())
        {
            GameEventDispatcher.DispatchOutOfAmmoWithRemainingTargets();
        }
    }
}
