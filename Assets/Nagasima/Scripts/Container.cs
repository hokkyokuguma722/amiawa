using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private PresenterChanger presenterChanger;

    public TitlePresenter titlePresenter;
    private TitleModel titleModel;
    [SerializeField]
    private TitleView titleView;
    [SerializeField]
    private InGameView inGameView;
    [SerializeField]
    private GameResultsView resultView;

    public InGamePresenter inGamePresenter;
    private InGameModel inGameModel;

    //InGameViewの中身が決まっていないため保留
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

        //InGameViewの中身が決まっていないため保留
        //inGamePresenter = new(inGameModel, InGameView, presenterChanger);
    }
}
