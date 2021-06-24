using UnityEngine;

public class RootBoneController : MonoBehaviour
{
    [SerializeField]
    private PartController _part;

    private bool _springPopAngleFlipped = true;

    private Transform _bodyReference;
    private HingeJoint _hingeJoint;
    private MeshFilter _mesh;

    [SerializeField]
    private Mesh _rightArm;
    [SerializeField]
    private Mesh _leftArm;

    private void Awake()
    {
        _bodyReference = GameObject.FindGameObjectWithTag("Body").transform;
        _hingeJoint = GetComponent<HingeJoint>();
        _mesh = GetComponentInChildren<MeshFilter>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x > _bodyReference.position.x)
        {
            if (_springPopAngleFlipped)
            {
                _hingeJoint.spring = new JointSpring() { damper = _hingeJoint.spring.damper, spring = _hingeJoint.spring.spring, targetPosition = _hingeJoint.spring.targetPosition * -1 };
                _springPopAngleFlipped = false;

                if (_mesh.gameObject.tag == "Arm")
                {
                    //apply right arm
                    _mesh.mesh = _leftArm;
                    _mesh.transform.Rotate(new Vector3(0, 0, 45));
                }
            }
        }
        else
        {
            if (!_springPopAngleFlipped)
            {
                _hingeJoint.spring = new JointSpring() { damper = _hingeJoint.spring.damper, spring = _hingeJoint.spring.spring, targetPosition = _hingeJoint.spring.targetPosition * -1 };
                _springPopAngleFlipped = true;

                if (_mesh.gameObject.tag == "Arm")
                {
                    //apply left arm
                    _mesh.mesh = _rightArm;
                    _mesh.transform.Rotate(new Vector3(0,0,-45));
                }
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
