using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    //枚举变量
    public enum Direction { up, down, right, left };
    public Direction direction;

    [Header("Room Info0")]
    public GameObject roomPrefab;
    public int roomNumber;
    public Color starColor, endColor;
    private Room endRoom;
    public LayerMask roomLayaer;

    [Header("Locate")]
    public Transform point;
    public float xOffset, yOffset;

    List<Room> rooms = new List<Room>();
    List<Vector3> roomsPosition = new List<Vector3>();

    //创建最终房
    public int maxStep;

    List<Room> farRoom = new List<Room>();
    List<Room> lessFarRoon = new List<Room>();
    List<Room> oneWayRoom = new List<Room>();

    //在下面创建的类
    public WallType walltype;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < roomNumber; i++)
        {
            /*while (roomsPosition.Contains(point.position))
            {
                //change room position;
                ChangePointPosition();
            }*/
            rooms.Add(Instantiate(roomPrefab, point.position, Quaternion.identity).GetComponent<Room>());
            roomsPosition.Add(point.position);
            ChangePointPosition();
        }

        //找最后一个生成房间
        endRoom = rooms[1];
        rooms[0].GetComponent<SpriteRenderer>().color = starColor;
        foreach (var r in rooms)
        {
            //比较房间距离
            /*if (r.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)
            {
                endRoom = r;
            }*/

            SetupDoor(r, r.transform.position);
            
        }
        FindEndRoom();
        endRoom.GetComponent<SpriteRenderer>().color = endColor;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ChangePointPosition()
    {
        do
        { 
            direction = (Direction)Random.Range(0, 4);

            switch (direction)
            {
                case Direction.up:
                    point.position += new Vector3(0, yOffset, 0);
                    break;

                case Direction.down:
                    point.position += new Vector3(0, -yOffset, 0);
                    break;

                case Direction.right:
                    point.position += new Vector3(-xOffset, 0, 0);
                    break;

                case Direction.left:
                    point.position += new Vector3(xOffset, 0, 0);
                    break;

            }
        } while (roomsPosition.Contains(point.position));
    }

    public void SetupDoor(Room room, Vector3 roomPosition)
    {
        room.hasUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayaer);
        room.hasDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayaer);
        room.hasLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset,0 , 0), 0.2f, roomLayaer);
        room.hasRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayaer);

        room.UpdateRoom(xOffset,yOffset);

        switch (room.doorNum)
        {
            case 1:
                if (room.hasUp)
                    Instantiate(walltype.sU, roomPosition, Quaternion.identity);
                if (room.hasDown)
                    Instantiate(walltype.sD, roomPosition, Quaternion.identity);
                if (room.hasLeft)
                    Instantiate(walltype.sL, roomPosition, Quaternion.identity);
                if (room.hasRight)
                    Instantiate(walltype.sR, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (room.hasUp && room.hasDown)
                    Instantiate(walltype.dUD, roomPosition, Quaternion.identity);
                if (room.hasUp && room.hasRight)
                    Instantiate(walltype.dUR, roomPosition, Quaternion.identity);
                if (room.hasUp && room.hasLeft)
                    Instantiate(walltype.dUL, roomPosition, Quaternion.identity);
                if (room.hasDown && room.hasRight)
                    Instantiate(walltype.dDR, roomPosition, Quaternion.identity);
                if (room.hasDown && room.hasLeft)
                    Instantiate(walltype.dDL, roomPosition, Quaternion.identity);
                if (room.hasRight && room.hasLeft)
                    Instantiate(walltype.dRL, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (!room.hasUp)
                    Instantiate(walltype.tDLR, roomPosition, Quaternion.identity);
                if (!room.hasDown)
                    Instantiate(walltype.tULR, roomPosition, Quaternion.identity);
                if (!room.hasRight)
                    Instantiate(walltype.tUDL, roomPosition, Quaternion.identity);
                if (!room.hasLeft)
                    Instantiate(walltype.tUDR, roomPosition, Quaternion.identity);

                break;
            case 4:
                Instantiate(walltype.f, roomPosition, Quaternion.identity);
                break;
        }
    }

    public void FindEndRoom()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;
        }
        foreach (var r in rooms)
        {
            if (r.stepToStart == maxStep)
                farRoom.Add(r);
            if (r.stepToStart == maxStep - 1)
                lessFarRoon.Add(r);
        }
        for (int i = 0; i < farRoom.Count; i++)
        {
            if (farRoom[i].doorNum == 1)
                oneWayRoom.Add(farRoom[i]);
        }
        for (int i = 0; i < lessFarRoon.Count; i++)
        {
            if (lessFarRoon[i].doorNum == 1)
                oneWayRoom.Add(lessFarRoon[i]);
        }

        if(oneWayRoom.Count != 0)
        {
            endRoom = oneWayRoom[Random.Range(0, oneWayRoom.Count)];
        }
        else
        {
            endRoom = farRoom[Random.Range(0, farRoom.Count)];
        }

    }
}
//没有使用monobehaviour，所以加这个system
[System.Serializable]
public class WallType
{
    public GameObject sU, sD, sL, sR, dUD, dUR, dUL, dDR, dDL,dRL,tUDR, tUDL, tULR,tDLR, f;

}