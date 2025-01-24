using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CounterScreenView : BaseScreenView
{
    [SerializeField] private TMP_Text _message;
    [SerializeField] private TMP_Text _counter;
    [SerializeField] private Button _incrementButton;
    [SerializeField] private Button _reloadButton;

    public Action OnIncrementClick;
    public Action OnReloadClick;

    private void OnEnable()
    {
        _incrementButton.onClick.AddListener(IncrementClick);
        _reloadButton.onClick.AddListener(ReloadClick);
    }

    private void OnDisable()
    {
        _incrementButton.onClick.RemoveListener(IncrementClick);
        _reloadButton.onClick.RemoveListener(ReloadClick);
    }

    public void Initialize(Sprite buttonSprite, string message, int startingNumber)
    {
        _incrementButton.image.sprite = buttonSprite;
        _message.text = message;
        _counter.text = startingNumber.ToString();
    }

    public void UpdateCounter(int count)
    {
        _counter.text = count.ToString();
    }

    private void IncrementClick()
    {
        OnIncrementClick?.Invoke();
    }

    private void ReloadClick()
    {
        OnReloadClick?.Invoke();
    }
}