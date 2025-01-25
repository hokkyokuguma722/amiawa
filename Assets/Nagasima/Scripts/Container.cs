using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

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

    private AudioSource audioSource;

    private GameResultsPresenter gameResultsPresenter;
    private GameResultsModel gameResultsModel;

    public void Initialize()
    {
        audioSource = GetComponent<AudioSource>();

        titleModel = new();
        inGameModel = new();
        gameResultsModel = new();
        presenterChanger = new();

        titlePresenter = new(titleModel, titleView, presenterChanger);
        inGamePresenter = new(inGameModel, inGameView, presenterChanger, audioSource);
        gameResultsPresenter = new(gameResultsModel, resultView, presenterChanger);


        Dictionary<string, IPresenter> presenterDictionary = new Dictionary<string, IPresenter>()
        {
            {
                "titlePresenter", titlePresenter
            },
            {
                "InGamePresenter", inGamePresenter
            },
            {
                "GameresultsPresenter", gameResultsPresenter
            }
        };
        presenterChanger.Initialize(presenterDictionary);

    }
}
