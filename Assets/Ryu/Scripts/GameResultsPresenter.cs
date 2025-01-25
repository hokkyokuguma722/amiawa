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
        SetButtonAciton();
    }
    public void Show()
    {
        gameResultsView.Show();
    }
    public void Hide()
    {
        gameResultsView.Hide();
    }

    public GameResultsPresenter(GameResultsModel model, GameResultsView view, PresenterChanger pChanger)
    {
        gameResultsModel = model;
        gameResultsView = view;
        presenterChanger = pChanger;
    }

    private void SetButtonAciton()
    {
        gameResultsView.onClickStartButtonAsObservale
            .Subscribe(_ =>
            {
                Debug.Log("�{�^���������ꂽ");
                presenterChanger.ChangePresenter("gamePresenter");
            })
        .AddTo(disposables);
    }
}
