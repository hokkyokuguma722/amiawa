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

    public IObservable<Unit> onClickStartButtonAsObservale => startButton.onClick.AsObservable();

    public void Hide()
    {
        titleImage.gameObject.SetActive(false);
    }

    public void Show()
    {
        titleImage.gameObject.SetActive(true);
    }

    public void Initialize()
    {
        
    }
}
