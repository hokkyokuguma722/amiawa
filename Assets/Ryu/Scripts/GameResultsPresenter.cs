using UniRx;

public class GameResultsPresenter : IPresenter
{
    private GameResultsModel gameResultsModel;
    private GameResultsView gameResultsView;
    private PresenterChanger presenterChanger;

    private CompositeDisposable disposables = new();

    public void Initialize()
    {
        SetButtonAction();
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

    private void SetButtonAction()
    {
        gameResultsView.onClickStartButtonAsObservale
            .Subscribe(_ => { presenterChanger.ChangePresenter("titlePresenter"); })
            .AddTo(disposables);
    }
}