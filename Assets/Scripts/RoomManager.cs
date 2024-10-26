using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomManager : MonoBehaviour
{
    public GameObject roomPrefab;
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public float roomWidth = 10f;
    public float roomHeight = 10f;
    public int maxRooms = 10;

    private int[,] roomGrid;
    private List<GameObject> roomObjects = new List<GameObject>();
    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    private int roomCount = 0;
    private bool generationComplete = false;

    public GameObject playerPrefab;
    private GameObject player;

    void Start()
    {
        roomGrid = new int[gridSizeX, gridSizeY];
        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);
    }

    void Update()
    {
        if (roomQueue.Count > 0 && roomCount < maxRooms && !generationComplete)
        {
            Vector2Int roomIndex = roomQueue.Dequeue();
            int gridX = roomIndex.x;
            int gridY = roomIndex.y;

            Debug.Log($"Processing room at index: {roomIndex}, Current room count: {roomCount}");

            bool roomAdded = false;

            roomAdded |= TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
            roomAdded |= TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
            roomAdded |= TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
            roomAdded |= TryGenerateRoom(new Vector2Int(gridX, gridY - 1));

            if (!roomAdded)
            {
                roomQueue.Enqueue(roomIndex);
            }
        }
        else if (!generationComplete)
        {
            generationComplete = true;
            Debug.Log("Room generation complete!");
            SpawnPlayer();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RegenerateRooms();
        }
    }

    private void StartRoomGenerationFromRoom(Vector2Int roomIndex)
    {
        roomQueue.Enqueue(roomIndex);
        int x = roomIndex.x;
        int y = roomIndex.y;
        roomGrid[x, y] = 1;
        roomCount++;
        var initialRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        initialRoom.name = $"Room-{roomCount}";
        Room roomScript = initialRoom.GetComponent<Room>();
        roomScript.Initialize(roomIndex, roomWidth, roomHeight);
        roomObjects.Add(initialRoom);
    }

    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        if (x >= gridSizeX || y >= gridSizeY || x < 0 || y < 0)
            return false;

        if (roomCount >= maxRooms)
            return false;

        if (roomGrid[x, y] != 0)
            return false;

        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
            return false;

        if (CountAdjacentRooms(roomIndex) > 1)
            return false;

        roomQueue.Enqueue(roomIndex);
        roomGrid[x, y] = 1;
        roomCount++;

        var newRoom = Instantiate(roomPrefab, GetPositionFromGridIndex(roomIndex), Quaternion.identity);
        Room roomScript = newRoom.GetComponent<Room>();
        roomScript.Initialize(roomIndex, roomWidth, roomHeight);
        newRoom.name = $"Room-{roomCount}";
        roomObjects.Add(newRoom);

        OpenDoors(newRoom, x, y);

        Debug.Log($"Generated room at index: {roomIndex}, Total rooms: {roomCount}");

        return true;
    }

    private void RegenerateRooms()
    {
        roomObjects.ForEach(Destroy);
        roomObjects.Clear();
        roomGrid = new int[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        Vector2Int initialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        StartRoomGenerationFromRoom(initialRoomIndex);

        if (player != null)
        {
            Destroy(player);
        }
    }

    void OpenDoors(GameObject room, int x, int y)
    {
        Room newRoomScript = room.GetComponent<Room>();

        Room leftRoomScript = GetRoomScriptAt(new Vector2Int(x - 1, y));
        Room rightRoomScript = GetRoomScriptAt(new Vector2Int(x + 1, y));
        Room topRoomScript = GetRoomScriptAt(new Vector2Int(x, y + 1));
        Room bottomRoomScript = GetRoomScriptAt(new Vector2Int(x, y - 1));

        if(x > 0 && roomGrid[x - 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.left);
            leftRoomScript?.OpenDoor(Vector2Int.right);
        }
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.right);
            rightRoomScript?.OpenDoor(Vector2Int.left);
        }
        if(y > 0 && roomGrid[x, y - 1] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.down);
            bottomRoomScript?.OpenDoor(Vector2Int.up);
        }
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0)
        {
            newRoomScript.OpenDoor(Vector2Int.up);
            topRoomScript?.OpenDoor(Vector2Int.down);
        }
    }

    Room GetRoomScriptAt(Vector2Int index)
    {
        GameObject roomObject = roomObjects.Find(r => r.GetComponent<Room>().RoomIndex == index);
        return roomObject?.GetComponent<Room>();
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;

        if (x > 0 && roomGrid[x - 1, y] != 0) count++;
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != 0) count++;
        if (y > 0 && roomGrid[x, y - 1] != 0) count++;
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != 0) count++;

        return count;
    }

    private Vector3 GetPositionFromGridIndex(Vector2Int gridIndex)
    {
        int gridX = gridIndex.x;
        int gridY = gridIndex.y;
        return new Vector3(roomWidth * (gridX - gridSizeX / 2),
            roomHeight * (gridY - gridSizeY / 2));
    }

    public Room GetRoomAtPosition(Vector3 position)
    {
        foreach (GameObject roomObject in roomObjects)
        {
            Room room = roomObject.GetComponent<Room>();
            if (room.RoomBounds.Contains(position))
            {
                return room;
            }
        }
        return null;
    }

    // ... (previous code remains the same)

private void SpawnPlayer()
{
    if (roomObjects.Count > 0 && playerPrefab != null)
    {
        GameObject initialRoom = roomObjects[0];
        Vector3 spawnPosition = initialRoom.transform.position;
        player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.SetRoomManager(this);
        }
        else
        {
            Debug.LogError("Player prefab does not have a Player component!");
        }
    }
}

    private void OnDrawGizmos()
    {
        Color gizmoColor = new Color(0, 1, 1, 0.05f);
        Gizmos.color = gizmoColor;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 position = GetPositionFromGridIndex(new Vector2Int(x, y));
                Gizmos.DrawWireCube(position, new Vector3(roomWidth, roomHeight, 1));
            }
        }
    }
}