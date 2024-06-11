using UnityEngine;

public class CameraController : MonoBehaviour
{

    private float speed = 1;

    private float progress;

    private Camera cam;

    private Vector3 targetPos;
    private Vector3 startPos;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        } 
        if (Input.GetMouseButton(0)) {
            targetPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            progress = Time.deltaTime * speed;
            transform.position = new Vector3(Mathf.LerpUnclamped(transform.position.x, transform.position.x-(targetPos.x-startPos.x), progress), transform.position.y, Mathf.LerpUnclamped(transform.position.z, transform.position.z - (targetPos.z - startPos.z), progress));
        }
    }
}