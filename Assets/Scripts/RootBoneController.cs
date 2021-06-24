using UnityEngine;

public class RootBoneController : MonoBehaviour
{
    [SerializeField]
    private PartController _part;

    private bool _springPopAngleFlipped = true;

    private Transform _bodyReference;
    private HingeJoint _hingeJoint;
    private MeshFilter _mesh;
    private MeshCollider _meshCollider;

    [SerializeField]
    private Mesh _rightArm;
    [SerializeField]
    private Mesh _leftArm;

    private float _springStart;

    private void Awake()
    {
        _bodyReference = GameObject.FindGameObjectWithTag("Body").transform;
        _hingeJoint = GetComponent<HingeJoint>();
        _mesh = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();

        _springStart = _hingeJoint.spring.targetPosition;
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
                    _meshCollider.sharedMesh = _leftArm;
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
                    _meshCollider.sharedMesh = _rightArm;
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

    public void SetScale(float scale)
    {
        //var positiveNegative = Mathf.Clamp(-1, 1, _hingeJoint.spring.targetPosition);
        //positiveNegative = Mathf.Ceil(positiveNegative);
        //var currentPosition = Mathf.Abs(_hingeJoint.spring.targetPosition);
        //_hingeJoint.spring = new JointSpring() { damper = _hingeJoint.spring.damper, spring = _hingeJoint.spring.spring, targetPosition = currentPosition * scale * positiveNegative };

        transform.localScale = new Vector3(scale, scale, scale);
    }
}
