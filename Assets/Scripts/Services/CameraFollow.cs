using UnityEngine;

public class CameraFollow:MonoBehaviour
{
    [SerializeField] private float smoothFactor;

    private Transform target;

    private Vector3 offset;

    public void Init(Transform target)
    {
        this.offset = transform.position - target.position;
        this.target = target;
    }

    private void Update()
    {
        if(target != null)
        {
            Vector3 newPos = Vector3.Lerp(transform.position - offset, target.position, 1 / smoothFactor * Time.deltaTime);
            transform.position = newPos + offset;
        }
            
    }
}