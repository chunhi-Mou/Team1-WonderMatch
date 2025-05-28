using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButtonUI : MonoBehaviour
{
    [SerializeField] new GameObject gameObject;
    private void OnEnable()
    {
        GameEvents.OnWinGame += HideButton;
        GameEvents.OnLoseGame += HideButton;
    }
    private void OnDisable()
    {
        GameEvents.OnWinGame -= HideButton;
        GameEvents.OnLoseGame -= HideButton;
    }
    private void HideButton()
    {
        gameObject.SetActive(false);
    }
}
