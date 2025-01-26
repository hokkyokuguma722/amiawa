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
        Debug.Log("TitlePresenter��������");
        SetButtonAction();
    }

    public void Show()
    {
        titleView.Show();
        // Debug.Log("�^�C�g����ʂ�\��");
    }

    public void Hide()
    {
        titleView.Hide();
        // Debug.Log("�^�C�g����ʂ��\��");
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
                Debug.Log("�{�^���������ꂽ");
                presenterChanger.ChangePresenter("InGamePresenter");
                SoundManager.instance.PalySE(0);
            })
            .AddTo(disposables);

        titleView.onClickQuitButtonAsObservable
            .Subscribe(_ =>
            {
                Debug.Log("Quit�{�^���������ꂽ");
                Application.Quit();
                SoundManager.instance.PalySE(1);
            })
            .AddTo(disposables);
        SoundManager.instance.PlayBGM(SceneType.ThridScene);
    }
}

