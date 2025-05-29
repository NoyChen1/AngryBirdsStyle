using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEventDispatcher
{
    public static event Action<Vector3> OnShoot;
    public static event Action<Vector3, Vector3> OnStartAiming;
    public static event Action<List<Vector3>> OnTrajectoryUpdated;
    public static event Action<GameObject, GameObject, float> OnProjectileHit;
    public static event Action<ProjectileType, int> OnAmmoChanged;
    public static event Action OnAmmoSwitchRequested;
    public static event Action OnAllTargetsEliminated;
    public static event Action OnOutOfAmmoWithRemainingTargets;
    public static event Action OnVictory;
    public static event Action OnDefeat;




    public static void DispatchShoot(Vector3 direction) => OnShoot?.Invoke(direction);
    public static void DispatchStartAiming(Vector3 origin, Vector3 direction) => OnStartAiming?.Invoke(origin, direction);
    public static void DispatchTrajectoryUpdated(List<Vector3> path) => OnTrajectoryUpdated?.Invoke(path);
    public static void DispatchProjectileHit(GameObject projectile, GameObject target, float damage) => 
                                                OnProjectileHit?.Invoke(projectile, target, damage);
    public static void DispatchAmmoChanged(ProjectileType type, int count) => OnAmmoChanged?.Invoke(type, count);

    public static void DispatchAmmoSwitchRequested() => OnAmmoSwitchRequested?.Invoke();

    public static void DispatchAllTargetsEliminated() => OnAllTargetsEliminated?.Invoke();
    public static void DispatchOutOfAmmoWithRemainingTargets() => OnOutOfAmmoWithRemainingTargets?.Invoke();

    public static void DispatchVictory() => OnVictory?.Invoke();
    public static void DispatchDefeat() => OnDefeat?.Invoke();


}
