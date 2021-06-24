using UnityEngine;

public class AnchorController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Part"))
        {
            var bone = other.gameObject.GetComponent<RootBoneController>();
            bone.OnTriggerEnterOther(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Part"))
        {
            var bone = other.gameObject.GetComponent<RootBoneController>();
            bone.OnTriggerExitOther(this);
        }
    }
}
