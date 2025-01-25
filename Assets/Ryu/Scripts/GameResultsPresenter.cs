using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class GameResultsPresenter : IPresenter
{
    private GameResultsModel gameResultsModel;
    private GameResultsView gameResultsView;
    private PresenterChanger presenterChanger;

    private CompositeDisposable disposables = new CompositeDisposable();

    public void Initialize()
    {
        Debug.Log("TitlePresenterを初期化");
        SetButtonAciton();
    }
    public void Show()
    {
        gameResultsView.Show();
        Debug.Log("タイトル画面を表示");
    }
    public void Hide()
    {
        gameResultsView.Hide();
        Debug.Log("タイトル画面を非表示");
    }

    public GameResultsPresenter(GameResultsModel model, GameResultsView view, PresenterChanger pChanger)
    {
        gameResultsModel = model;
        gameResultsView = view;
        presenterChanger = pChanger;

        gameResultsView.Show();
    }

    private void SetButtonAciton()
    {
        gameResultsView.onClickStartButtonAsObservale
            .Subscribe(_ =>
            {
                Debug.Log("ボタンが押された");
                presenterChanger.ChangePresenter("gamePresenter");
            })
        .AddTo(disposables);
    }
}
