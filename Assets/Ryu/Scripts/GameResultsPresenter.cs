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
        Debug.Log("TitlePresenter��������");
        SetButtonAciton();
    }
    public void Show()
    {
        gameResultsView.Show();
        Debug.Log("�^�C�g����ʂ�\��");
    }
    public void Hide()
    {
        gameResultsView.Hide();
        Debug.Log("�^�C�g����ʂ��\��");
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
                Debug.Log("�{�^���������ꂽ");
                presenterChanger.ChangePresenter("gamePresenter");
            })
        .AddTo(disposables);
    }
}
