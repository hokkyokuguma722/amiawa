using UnityEngine;

public class InGamePresenter : IPresenter
{
    private InGameModel inGameModel;
    private IInGameView inGameView;
    private PresenterChanger presenterChanger;

    public InGamePresenter(InGameModel model, IInGameView view, PresenterChanger pChanger)
    {
        inGameModel = model;
        inGameView = view;
        presenterChanger = pChanger;
    }

    public void Initialize()
    {
        Debug.Log("InGamePresenterを初期化");
    }

    public void Show()
    {
        Debug.Log("インゲーム画面を表示");
        inGameView.Show();
    }

    public void Hide()
    {
        Debug.Log("インゲーム画面を非表示");
        inGameView.Hide();
    }
}
