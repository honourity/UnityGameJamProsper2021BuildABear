using UnityEngine;

public class RootBoneController : MonoBehaviour
{
    [SerializeField]
    private PartController _part;

    private bool _springPopAngleFlipped;

    private Transform _bodyReference;
    private HingeJoint _hingeJoint;

    private void Awake()
    {
        _bodyReference = GameObject.FindGameObjectWithTag("Body").transform;
        _hingeJoint = GetComponent<HingeJoint>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x > _bodyReference.position.x)
        {
            if (_springPopAngleFlipped)
            {
                _hingeJoint.spring = new JointSpring() { damper = _hingeJoint.spring.damper, spring = _hingeJoint.spring.spring, targetPosition = _hingeJoint.spring.targetPosition * -1 };
                _springPopAngleFlipped = false;
            }
        }
        else
        {
            if (!_springPopAngleFlipped)
            {
                _hingeJoint.spring = new JointSpring() { damper = _hingeJoint.spring.damper, spring = _hingeJoint.spring.spring, targetPosition = _hingeJoint.spring.targetPosition * -1 };
                _springPopAngleFlipped = true;
            }
        }
    }

    public void OnMouseDown()
    {
        _part.OnMouseDown();
    }

    public void OnMouseDrag()
    {
        _part.OnMouseDrag();
    }

    public void OnMouseUp()
    {
        _part.OnMouseUp();
    }

    public void OnTriggerEnterOther(AnchorController anchorController)
    {
        _part.OnTriggerEnterOther(anchorController);
    }

    public void OnTriggerExitOther(AnchorController anchorController)
    {
        _part.OnTriggerExitOther(anchorController);
    }
}
