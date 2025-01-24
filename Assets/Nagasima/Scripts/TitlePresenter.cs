using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class TitlePresenter : IPresenter
{
    private TitleModel TitleModel;
    private TitleView TitleView;

    private CompositeDisposable disposables= new CompositeDisposable();

    public void Initialize()
    {
        Debug.Log("TitlePresenter‚ð‰Šú‰»");
        SetButtonAction();
    }

    public void Show()
    {
        TitleView.Show();
    }

    public void Hide()
    {
        TitleView.Hide();
    }

    public TitlePresenter(TitleModel model, TitleView view)
    {
        TitleModel = model;
        TitleView = view;

        TitleView.Show();
    }

    private void SetButtonAction()
    {
        TitleView.onClickStartButtonAsObservale
            .Subscribe(_ =>
            {
                Debug.Log("ƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚½");
            })
            .AddTo(disposables);
    }
}

