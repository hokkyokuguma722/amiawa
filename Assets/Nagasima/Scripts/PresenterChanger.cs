using System.Collections.Generic;
using UnityEngine;

public class PresenterChanger
{
   private Dictionary<string, IPresenter> presenterDictionary=new();

    private IPresenter currentPresenter;
    private IPresenter nextPresenter;

    public void Initialize(Dictionary<string, IPresenter> dictionary)
    {
        presenterDictionary = dictionary;
    }

    public void ChangePresenter(string presenterName)
    {
        nextPresenter = presenterDictionary[presenterName];
        currentPresenter.Hide();
        nextPresenter.Show();
        currentPresenter = nextPresenter;
    }
}
