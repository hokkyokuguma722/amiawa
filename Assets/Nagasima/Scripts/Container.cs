using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public TitlePresenter titlePresenter;
    private TitleModel titleModel;
    [SerializeField]
    private TitleView titleView;

    private PresenterChanger presenterChanger;

    public void Initialize() 
    { 
        titleModel = new();

        presenterChanger = new();
        Dictionary<string, IPresenter> presenterDictionary = new Dictionary<string, IPresenter>()
        {
            {
                "titlePresenter", titlePresenter
            }
        };
        presenterChanger.Initialize(presenterDictionary);

        titlePresenter = new(titleModel, titleView, presenterChanger);
    }
}
