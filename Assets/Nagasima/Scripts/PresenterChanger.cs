using System.Collections.Generic;

public class PresenterChanger
{
    private Dictionary<string, IPresenter> presenterDictionary = new();

    private IPresenter currentPresenter;
    private IPresenter nextPresenter;

    public void Initialize(Dictionary<string, IPresenter> dictionary)
    {
        presenterDictionary = dictionary;
        currentPresenter = presenterDictionary.GetValueOrDefault("titlePresenter");
    }

    public void ChangePresenter(string presenterName)
    {
        nextPresenter = presenterDictionary.GetValueOrDefault(presenterName);
        currentPresenter.Hide();
        nextPresenter.Initialize();
        nextPresenter.Show();
        currentPresenter = nextPresenter;
    }
}
