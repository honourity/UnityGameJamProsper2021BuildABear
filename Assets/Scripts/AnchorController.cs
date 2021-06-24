using System.Collections;
using UnityEngine;

public class AnchorController : MonoBehaviour
{
    private Collider _collider;
    private SpriteRenderer _indicator;
    private Vector3 _indicatorStartingScale;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _indicator = GetComponentInChildren<SpriteRenderer>();
        _indicatorStartingScale = _indicator.transform.localScale;
    }

    public void EnableAnchor()
    {
        _collider.enabled = true;
        StartCoroutine(PulseCoroutine());
    }

    public void DisableAnchor()
    {
        _indicator.enabled = false;

        _collider.enabled = false;
        StopAllCoroutines();
    }

    private IEnumerator PulseCoroutine()
    {
        _indicator.enabled = true;
        _indicator.transform.localScale = _indicatorStartingScale;

        var scalingFactor = 2f;
        var counter = 0f;

        while (true)
        {
            counter += Time.deltaTime * 2f;
            var scale = Mathf.PingPong(counter, _indicatorStartingScale.x / scalingFactor) + _indicatorStartingScale.x / (2 * scalingFactor);

            _indicator.transform.localScale = Vector3.one * scale;

            yield return new WaitForEndOfFrame();
        }
    }

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
