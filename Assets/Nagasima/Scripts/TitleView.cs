using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TitleView : MonoBehaviour, IView
{
    [SerializeField]
    private Image titleImage;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button quitButton;

    public IObservable<Unit> onClickStartButtonAsObservable => startButton.onClick.AsObservable();
    public IObservable<Unit> onClickQuitButtonAsObservable => quitButton.onClick.AsObservable();

    public void Initialize()
    {

    }

    public void Show()
    {
        titleImage.gameObject.SetActive(true);
    }

    public void Hide()
    {
        titleImage.gameObject.SetActive(false);
    }
}
