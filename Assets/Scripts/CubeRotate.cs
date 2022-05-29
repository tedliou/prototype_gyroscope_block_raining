using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    public bool isBlock;
    public float rotateSpeed;
    public new Rigidbody rigidbody;

    private void OnValidate()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var ray = new Ray(transform.position, Vector3.up);
        var colliders = Physics.RaycastAll(ray, 10);
        foreach (var e in colliders)
        {
            if (e.transform.CompareTag("Block"))
            {
                isBlock = true;
                return;
            }
        }

        isBlock = false;
    }

    private void FixedUpdate()
    {
        rigidbody.MoveRotation(Quaternion.Euler(transform.eulerAngles + new Vector3(0, rotateSpeed * (isBlock ? 4 : 1), 0)));
    }
}
