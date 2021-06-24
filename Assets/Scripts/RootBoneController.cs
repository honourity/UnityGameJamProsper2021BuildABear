using System;
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

    private void Awake()
    {
        _bodyReference = GameObject.FindGameObjectWithTag("Body").transform;
        _hingeJoint = GetComponent<HingeJoint>();
        _mesh = GetComponent<MeshFilter>();
        _meshCollider = GetComponent<MeshCollider>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x > _bodyReference.position.x && _mesh.gameObject.tag != "Head")
        {
            if (_springPopAngleFlipped)
            {
                _hingeJoint.spring = new JointSpring() { damper = _hingeJoint.spring.damper, spring = _hingeJoint.spring.spring, targetPosition = _hingeJoint.spring.targetPosition * -1 };
                _springPopAngleFlipped = false;

                if (_mesh.gameObject.tag == "Arm" || _mesh.gameObject.tag == "Leg")
                {
                    //apply right arm
                    _mesh.mesh = _leftArm;
                    _meshCollider.sharedMesh = _leftArm;
                }
            }
        }
        else if (_mesh.gameObject.tag != "Head")
        {
            if (!_springPopAngleFlipped)
            {
                _hingeJoint.spring = new JointSpring() { damper = _hingeJoint.spring.damper, spring = _hingeJoint.spring.spring, targetPosition = _hingeJoint.spring.targetPosition * -1 };
                _springPopAngleFlipped = true;

                if (_mesh.gameObject.tag == "Arm" || _mesh.gameObject.tag == "Leg")
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
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetMesh(Mesh mesh)
    {
        _mesh.mesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }
}
