using UnityEngine;

public class InGamePresenter : IPresenter
{
    private InGameModel inGameModel;
    private InGameView inGameView;
    private PresenterChanger presenterChanger;

    public InGamePresenter(InGameModel model, InGameView view, PresenterChanger pChanger)
    {
        inGameModel = model;
        inGameView = view;
        presenterChanger = pChanger;

        inGameView.Show();
    }

    public void Initialize()
    {
        Debug.Log("InGamePresenterを初期化");
    }

    public void Show()
    {
        Debug.Log("インゲーム画面を表示");
        inGameView.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("インゲーム画面を非表示");
        inGameView.gameObject.SetActive(false);
    }
}
