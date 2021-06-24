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
    }

    private void Update()
    {
        if (_anchored)
        {
            SetToAnchor();
        }
    }

    public void OnMouseDrag()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, MouseBackingLayer, QueryTriggerInteraction.Collide))
        {
            if (_closeAnchor != null && Vector3.Distance(_closeAnchor.transform.position, hit.point) < SnapDistance)
            {
                SetToAnchor();
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
        foreach (var collider in GameManager.Instance.AnchorTriggers)
        {
            collider.enabled = true;
        }
    }

    public void OnMouseUp()
    {
        //disable all anchor triggers
        foreach (var collider in GameManager.Instance.AnchorTriggers)
        {
            collider.enabled = false;
        }

        if (_closeAnchor)
        {
            SetToAnchor();
        }
        else
        {
            Destroy(RootBone.gameObject);
            Destroy(gameObject);
        }
    }

    private void SetToAnchor()
    {
        if (_closeAnchor != null)
        {
            gameObject.transform.position = _closeAnchor.transform.position;
            _anchored = true;
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
}
