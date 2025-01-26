using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class GameResultsView : MonoBehaviour, IView
{
    [SerializeField] private Image titleImage;

    [SerializeField] private Button startButton;

    public IObservable<Unit> onClickStartButtonAsObservale => startButton.onClick.AsObservable();

    public void Initialize()
    {
    }

    public void Show()
    {
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}