using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI resultText;

    private void Awake()
    {
        panel.SetActive(false);
    }

    private void OnEnable()
    {
        GameEventDispatcher.OnVictory += ShowVictory;
        GameEventDispatcher.OnDefeat += ShowDefeat;

    }

    private void OnDisable()
    {
        GameEventDispatcher.OnVictory -= ShowVictory;
        GameEventDispatcher.OnDefeat -= ShowDefeat;

    }

    private void ShowVictory()
    {
        panel.SetActive(true);
        resultText.text = "Victory ! all target eliminated !";
    }

    private void ShowDefeat()
    {
        panel.SetActive(true);
        resultText.text = "You are out of Ammo ! Try Again !";
    }

}