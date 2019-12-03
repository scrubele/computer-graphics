using UnityEngine;
using UnityEngine.Serialization;

public class Scene : MonoBehaviour
{
    [SerializeField] public GameConfig _config;

    private IConfigProvider _configProvider;
    [FormerlySerializedAs("ButtonBackgroundPrefab")] public GameBackgroundItem buttonBackgroundPrefab;
    [FormerlySerializedAs("SceneBackgroundPrefab")] public GameBackgroundItem sceneBackgroundPrefab;
    [FormerlySerializedAs("SquareBackgroundPrefab")] public GameBackgroundItem squareBackgroundPrefab;
    [FormerlySerializedAs("Text2048")] public GameBackgroundItem text2048;
    public GameConfig Config => _config;

    public void Awake()
    {
        _configProvider = GetComponentInParent<IConfigProvider>();
        var sceneBackground = Instantiate(sceneBackgroundPrefab, transform);
        sceneBackground.position = new Vector3(1.5f, 2, 3);
        var squareBackground = Instantiate(squareBackgroundPrefab, transform);
        squareBackground.position = new Vector3(1.5f, 1.5f, 2);
        var buttonBackground = Instantiate(buttonBackgroundPrefab, transform);
        buttonBackground.position = new Vector3(1.35f, 4.5f, 2);
        var text2048 = Instantiate(this.text2048, transform);
        text2048.position = new Vector3(0.1f, 4.5f, 2);
    }
}