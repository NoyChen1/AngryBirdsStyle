using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool _gameEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _gameEnded = false ;
    }

    private void OnEnable()
    {
        GameEventDispatcher.OnAllTargetsEliminated += HandleVictory;
        GameEventDispatcher.OnOutOfAmmoWithRemainingTargets += HandleDefeat;
    }

    private void OnDisable()
    {
        GameEventDispatcher.OnAllTargetsEliminated -= HandleVictory;
        GameEventDispatcher.OnOutOfAmmoWithRemainingTargets -= HandleDefeat;
    }

    private void HandleVictory()
    {
        if (_gameEnded) return;

        _gameEnded = true;
        Debug.Log("Victory ! all target eliminated !");
        GameEventDispatcher.DispatchVictory();
    }

    private void HandleDefeat()
    {
        if (_gameEnded) return;

        _gameEnded = true;
        Debug.Log("You are out of Ammo ! Try Again !");
        GameEventDispatcher.DispatchDefeat();
    }
}

