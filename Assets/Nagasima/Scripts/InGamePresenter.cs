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
        Debug.Log("InGamePresenter��������");
    }

    public void Show()
    {
        Debug.Log("�C���Q�[����ʂ�\��");
        inGameView.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("�C���Q�[����ʂ��\��");
        inGameView.gameObject.SetActive(false);
    }
}
