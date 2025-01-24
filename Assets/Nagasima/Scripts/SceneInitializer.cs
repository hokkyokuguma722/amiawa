using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField]
    private Container container;

    private void Start()
    {
        container.Initialize();

        container.titlePresenter.Initialize();
    }
}
