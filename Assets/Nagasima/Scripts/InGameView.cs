using UnityEngine;

public interface IInGameView : IView
{

}

public class InGameView : MonoBehaviour, IInGameView
{
    public void Initialize()
    {
        
    }

    public void Show()
    {

    }

    public void Hide()
    {
        
    }
}
