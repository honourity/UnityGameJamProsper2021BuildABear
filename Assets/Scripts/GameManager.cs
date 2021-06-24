using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get { return _instance = _instance ?? FindObjectOfType<GameManager>() ?? new GameManager { }; }
    }
    private static GameManager _instance;

    public LayerMask MouseClickLayerMask;

    public AnchorController[] Anchors { get; set; }

    public SliderController Slider { get; set; }

    public AudioSource AudioSource { get; set; }

    private void Awake()
    {
        Anchors = FindObjectsOfType<AnchorController>();

        Camera.main.eventMask = MouseClickLayerMask;

        Slider = FindObjectOfType<SliderController>();

        AudioSource = GetComponent<AudioSource>();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
