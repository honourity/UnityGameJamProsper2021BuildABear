using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get { return _instance = _instance ?? FindObjectOfType<GameManager>() ?? new GameManager { }; }
    }
    private static GameManager _instance;

    public LayerMask MouseClickLayerMask;

    public Collider[] AnchorTriggers { get; set; }

    public SliderController Slider { get; set; }

    private void Awake()
    {
        AnchorTriggers = FindObjectsOfType<AnchorController>().Select(a => a.GetComponent<Collider>()).ToArray();

        Camera.main.eventMask = MouseClickLayerMask;

        Slider = FindObjectOfType<SliderController>();
    }
}
