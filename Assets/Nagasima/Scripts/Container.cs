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

    //InGameView�̒��g�����܂��Ă��Ȃ����ߕۗ�
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

        //InGameView�̒��g�����܂��Ă��Ȃ����ߕۗ�
        //inGamePresenter = new(inGameModel, InGameView, presenterChanger);
    }
}
