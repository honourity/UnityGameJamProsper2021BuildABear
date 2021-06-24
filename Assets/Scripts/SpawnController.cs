using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public PartController Part;

    private PartController _partInstance;

    private void OnMouseDown()
    {
        _partInstance = Instantiate(Part, transform.position, Quaternion.identity, null);
        _partInstance.RootBone.OnMouseDown();
    }

    private void OnMouseDrag()
    {
        _partInstance.RootBone.OnMouseDrag();
    }

    private void OnMouseUp()
    {
        _partInstance.RootBone.OnMouseUp();
    }
}
