using UnityEngine;


public class BowlerTarget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isBatsmanScene;
    [SerializeField] private Vector2 minMaxX;
    [SerializeField] private Vector2 minMaxZ;
    [SerializeField] private Vector2 moveSpeed;
    private Vector3  clickedPosition;
    private Vector3 clickedTargetPosition;

    private bool canMove;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !isBatsmanScene) 
            ManageControl(); 
    }

    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedPosition = Input.mousePosition;
            clickedTargetPosition = transform.position;
        }
        else if (Input.GetMouseButton(0)){
            Vector3 difference= Input.mousePosition - clickedPosition;

            difference.x /=Screen.width;
            difference.y /=Screen.height;

            Vector3 targetPosition = clickedTargetPosition + new Vector3 (difference.x * moveSpeed.x,0,difference.y * moveSpeed.y);
            targetPosition.x = Mathf .Clamp(targetPosition.x, minMaxX.x,minMaxX.y);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minMaxZ.x, minMaxZ.y);

            transform.position = targetPosition;
        }
    }

    public void Move(Vector2 movement)
    {
        float xPosition = Mathf .Lerp(minMaxX.x, minMaxX.y, movement.x);
        float zPosition = Mathf.Lerp(minMaxZ.x, minMaxZ.y, movement.y);


        transform.position = new Vector3(xPosition, 0, zPosition);
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;

    }
}
