using System;
using UnityEngine.UI;
using UnityEngine;
using UniRx;

public class GameResultsView : MonoBehaviour, IView
{
    [SerializeField]
    private Image titleImage;

    [SerializeField]
    private Button startButton;

    public IObservable<Unit> onClickStartButtonAsObservale => startButton.onClick.AsObservable();

    public void Initialize()
    {
       
    }

    public void Show()
    {
        titleImage.gameObject.SetActive(false);
    }
    public void Hide()
    {
        titleImage.gameObject.SetActive(false);
    }

}
