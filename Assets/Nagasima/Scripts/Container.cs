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

    //InGameView‚Ì’†g‚ªŒˆ‚Ü‚Á‚Ä‚¢‚È‚¢‚½‚ß•Û—¯
    //[SerializeField]
    //private InGameView inGameView;

    public void Initialize() 
    { 
        audioSource = GetComponent<AudioSource>();

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

        //InGameView‚Ì’†g‚ªŒˆ‚Ü‚Á‚Ä‚¢‚È‚¢‚½‚ß•Û—¯
        //inGamePresenter = new(inGameModel, InGameView, presenterChanger);
    }
}
