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
        Debug.Log("InGamePresenter��������");
    }

    public void Show()
    {
        Debug.Log("�C���Q�[����ʂ�\��");
        inGameView.Show();
    }

    public void Hide()
    {
        Debug.Log("�C���Q�[����ʂ��\��");
        inGameView.Hide();
    }
}
