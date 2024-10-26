using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int RoomIndex { get; set; }
    private SpriteRenderer spriteRenderer;
    public Bounds RoomBounds { get; private set; }

    [SerializeField] private GameObject[] doors;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on Room object!");
        }
        else
        {
            RoomBounds = spriteRenderer.bounds;
        }
    }

    public void Initialize(Vector2Int index, float width, float height)
    {
        RoomIndex = index;
        name = $"Room_{index.x}_{index.y}";
        SetRandomColor();
        RoomBounds = new Bounds(transform.position, new Vector3(width, height, 1));
    }

    private void SetRandomColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        }
    }

    public void OpenDoor(Vector2Int direction)
    {
        int doorIndex = GetDoorIndex(direction);
        if (doorIndex != -1 && doorIndex < doors.Length)
        {
            doors[doorIndex].SetActive(false);
        }
    }

    private int GetDoorIndex(Vector2Int direction)
    {
        if (direction == Vector2Int.up) return 0;
        if (direction == Vector2Int.right) return 1;
        if (direction == Vector2Int.down) return 2;
        if (direction == Vector2Int.left) return 3;
        return -1;
    }
}