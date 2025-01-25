using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private PresenterChanger presenterChanger;

    public TitlePresenter titlePresenter;
    private TitleModel titleModel;
    [SerializeField]
    private TitleView titleView;

    public InGamePresenter inGamePresenter;
    private InGameModel inGameModel;
    //[SerializeField]
    //private InGameView inGameView;

    public void Initialize() 
    { 
        titleModel = new();

        presenterChanger = new();
        Dictionary<string, IPresenter> presenterDictionary = new Dictionary<string, IPresenter>()
        {
            {
                "titlePresenter", titlePresenter
            },
            {
                "InGamePresenter", inGamePresenter
            }
        };
        presenterChanger.Initialize(presenterDictionary);

        titlePresenter = new(titleModel, titleView, presenterChanger);
        //inGamePresenter = new(inGameModel, InGameView, presenterChanger);
    }
}
