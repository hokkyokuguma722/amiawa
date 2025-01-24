using UnityEngine;

public class Container : MonoBehaviour
{
    public TitlePresenter titlePresenter;
    private TitleModel titleModel;
    [SerializeField]
    private TitleView titleView;

    public void Initialize() 
    { 
        titleModel = new();
        titlePresenter = new(titleModel, titleView);
    }
}
