using UnityEngine;

public class PartController : MonoBehaviour
{
    public LayerMask MouseBackingLayer;
    public float SnapDistance;
    public RootBoneController RootBone;

    [SerializeField]
    private AudioClip _attachSound;
    [SerializeField]
    private AudioClip _detachSound;

    private AnchorController _closeAnchor;
    private bool _anchored;

    private bool _notched;

    [SerializeField]
    private Mesh[] _notchedMeshes;

    private void Awake()
    {
        RootBone.transform.parent = null;
        GameManager.Instance.Slider.Deactivate();

        _notched = RootBone.CompareTag("Head");
    }

    private void Update()
    {
        if (_anchored)
        {
            SetToAnchor(false);
        }
    }

    public void OnMouseDrag()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, MouseBackingLayer, QueryTriggerInteraction.Collide))
        {
            if (_closeAnchor != null && Vector3.Distance(_closeAnchor.transform.position, hit.point) < SnapDistance)
            {
                SetToAnchor(false);
            }
            else
            {
                if (_anchored)
                {
                    PlayDetachEffect();
                    GameManager.Instance.Slider.Deactivate();
                }

                _anchored = false;
                gameObject.transform.position = hit.point;
            }
        }
    }

    public void OnMouseDown()
    {
        //enable all anchor triggers
        foreach (var anchor in GameManager.Instance.Anchors)
        {
            anchor.EnableAnchor();
        }

        if (_anchored)
        {
            GameManager.Instance.Slider.Deactivate();
        }
    }

    public void OnMouseUp()
    {
        //disable all anchor triggers
        foreach (var anchor in GameManager.Instance.Anchors)
        {
            anchor.DisableAnchor();
        }

        if (_closeAnchor)
        {
            SetToAnchor(true);
        }
        else
        {
            GameManager.Instance.Slider.Deactivate();
            Destroy(RootBone.gameObject);
            Destroy(gameObject);
        }
    }

    private void SetToAnchor(bool deliberate)
    {
        if (_closeAnchor != null)
        {
            gameObject.transform.position = _closeAnchor.transform.position;

            if (!_anchored)
            {
                PlayAttachEffect();
            }

            if (deliberate)
            {
                GameManager.Instance.Slider.SetNotched(_notched, _notchedMeshes?.Length);
                GameManager.Instance.Slider.Activate(OnSliderValueChanged);
            }

            _anchored = true;
        }
    }

    public void OnSliderValueChanged()
    {
        if (_notched)
        {
            RootBone.SetMesh(_notchedMeshes[Mathf.RoundToInt(GameManager.Instance.Slider.GetValue())]);
        }
        else
        {
            var scale = 2 * GameManager.Instance.Slider.GetValue();
            RootBone.SetScale(scale);
        }
    }

    public void OnTriggerEnterOther(AnchorController anchorController)
    {
        if (!_anchored)
        {
            _closeAnchor = anchorController;
        }
    }

    public void OnTriggerExitOther(AnchorController anchorController)
    {
        if (anchorController == _closeAnchor)
        {
            _closeAnchor = null;
        }
    }

    private void PlayAttachEffect()
    {
        GameManager.Instance.AudioSource.PlayOneShot(_attachSound);
    }

    private void PlayDetachEffect()
    {
        GameManager.Instance.AudioSource.PlayOneShot(_detachSound);
    }
}
