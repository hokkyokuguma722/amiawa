using UniRx;
public enum SceneType
{ 
    FristScene,
    SecondScene,
    ThridScene,
    ForthScene
}

public class InGameModel 
{
    public ReactiveProperty<SceneType> currentSceneType = new(SceneType.FristScene);
}
