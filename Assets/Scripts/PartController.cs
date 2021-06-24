using UnityEngine;

public class PartController : MonoBehaviour
{
    public LayerMask MouseBackingLayer;
    public float SnapDistance;
    public RootBoneController RootBone;

    private AnchorController _closeAnchor;
    private bool _anchored;

    private void Awake()
    {
        RootBone.transform.parent = null;
        GameManager.Instance.Slider.Deactivate();
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
            _anchored = true;

            if (deliberate)
            {
                GameManager.Instance.Slider.Activate(OnSliderValueChanged);
            }
        }
    }

    public void OnSliderValueChanged()
    {
        var scale = 2 * GameManager.Instance.Slider.GetValue();
        RootBone.SetScale(scale);
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
}
