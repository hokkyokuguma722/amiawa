using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class TitlePresenter : IPresenter
{
    private TitleModel titleModel;
    private TitleView titleView;
    private PresenterChanger presenterChanger;

    private CompositeDisposable disposables= new CompositeDisposable();

    public void Initialize()
    {
        Debug.Log("TitlePresenterを初期化");
        SetButtonAction();
    }

    public void Show()
    {
        titleView.Show();
        Debug.Log("タイトル画面を表示");
    }

    public void Hide()
    {
        titleView.Hide();
        Debug.Log("タイトル画面を非表示");
    }

    public TitlePresenter(TitleModel model, TitleView view, PresenterChanger pChanger)
    {
        titleModel = model;
        titleView = view;
        presenterChanger = pChanger;
    }

    private void SetButtonAction()
    {
        titleView.onClickStartButtonAsObservable
            .Subscribe(_ =>
            {
                Debug.Log("ボタンが押された");
                presenterChanger.ChangePresenter("InGamePresenter");
            })
            .AddTo(disposables);

        titleView.onClickQuitButtonAsObservable
            .Subscribe(_ =>
            {
                Debug.Log("Quitボタンが押された");
                Application.Quit();
            })
            .AddTo(disposables);
    }
}

