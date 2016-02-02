using UnityEngine;

using System;
using System.Collections.Generic;

using Base;
using GameLogic;
using StaticData;

public class MazeMain : MonoBehaviour
{
	public GameObject PassageOne;
	public GameObject PassageTwoLine;
	public GameObject PassageTwoTurn;
	public GameObject PassageThree;
	public GameObject PassageFour;

	public GameObject RoomA;
	public GameObject RoomB;
	public GameObject RoomC;
	public Transform BlockRoot;
	
	public GameObject CubePrefab;
	public GameObject LinkPrefab;
	public GameObject RoomPrefab;
	public Transform CubeRoot;

	private MazeBuilder builder;

	private const float BLOCK_SIZE = 10f;

	private const float CUBE_SIZE = 1.5f;
	private const float LINK_SIZE = 1f;
	private const float ROOM_COLS = 7f;
	private const float ROOM_ROWS = 7f;

	private MazeData mazeData;

	private MazeTable mazeTable;

	void Awake()
	{
		ConfigManager.Instance.InitAllData();

		mazeData = new MazeData();
		mazeData.Cols = 30;
		mazeData.Rows = 30;
		mazeData.StartCol = 15;
		mazeData.StartRow = 15;
		mazeData.LinkRate = 0.5f;
		mazeData.PassageRate = 0.9f;

		builder = new MazeBuilder(mazeData);
		mazeTable = builder.Build();
	}

	void Start()
	{
		for (int i = 0; i < mazeTable.Cols; ++i)
		{
			for (int j = 0; j < mazeTable.Rows; ++j)
			{
				MazeNode node = mazeTable.GetNode(i, j);
				if (node != null)
				{
//					Debug.Log((node.Col * 3).ToString() + ", " + (node.Row * 3).ToString() + "  " + node.Up.ToString() + ", " + node.Right.ToString() + ", " + node.Down.ToString() + ", " + node.Left.ToString());
					if (node is MazeRoom)
					{
						MazeRoom room = node as MazeRoom;
						if (!room.HasCreated)
						{
							room.HasCreated = true;
							BuildRoomBlock(room);
							BuildRoomCube(room);
						}
					}
					else
					{
						BuildPassageBlock(node);
						BuildPassageCube(node);
					}
				}
			}
		}
		
	}
	private void BuildRoomBlock(MazeRoom room)
	{
		BlockData data = BlockDataManager.Instance.GetRandomRoomData();
		GameObject roomGO = CreateRoomGO(data.GetResPath()); 
		roomGO.transform.localEulerAngles = Vector3.up * 90f * room.Direction;
		roomGO.transform.position = new Vector3(room.Col * BLOCK_SIZE , 0, room.Row * BLOCK_SIZE);
		roomGO.transform.parent = BlockRoot;
	}
	private void BuildRoomCube(MazeRoom room)
	{
		GameObject cube = Instantiate(RoomPrefab) as GameObject;
		BlockData data = BlockDataManager.Instance.GetRandomRoomData();
		cube.transform.localScale = new Vector3(data.Cols, 1, data.Rows);
		cube.transform.localEulerAngles = Vector3.up * 90f * room.Direction;
		cube.transform.position = new Vector3(room.Col * CUBE_SIZE , 0, room.Row * CUBE_SIZE);
		cube.transform.parent = CubeRoot;
	}
	private void BuildPassageBlock(MazeNode node)
	{
		PassageType type = MazeUtil.GetPassageType(node);
		GameObject passage = CreatePassageGO(type);
		passage.transform.position = new Vector3(node.Col * BLOCK_SIZE, 0, node.Row * BLOCK_SIZE);
		int direction = MazeUtil.GetPassageDirection(node, type);
		passage.transform.localEulerAngles = Vector3.up * direction * 90f;
		passage.transform.parent = BlockRoot;
	}
	private void BuildPassageCube(MazeNode node)
	{
		GameObject cube = Instantiate(CubePrefab) as GameObject;
		cube.transform.position = new Vector3(node.Col * CUBE_SIZE, 0, node.Row * CUBE_SIZE);
		cube.transform.parent = CubeRoot;
		
		GameObject link = null;
		if (node.LinkList[0] != null)
		{
			link = Instantiate(LinkPrefab) as GameObject;
			link.transform.localEulerAngles = Vector3.up * 90;
			link.transform.position = new Vector3(node.Col * CUBE_SIZE, 0, node.Row * CUBE_SIZE + LINK_SIZE);
			link.transform.parent = CubeRoot;
		}
		if (node.LinkList[1] != null)
		{
			link = Instantiate(LinkPrefab) as GameObject;
			link.transform.position = new Vector3(node.Col * CUBE_SIZE + LINK_SIZE, 0, node.Row * CUBE_SIZE);
			link.transform.parent = CubeRoot;
		}
		if (node.LinkList[2] != null)
		{
			link = Instantiate(LinkPrefab) as GameObject;
			link.transform.localEulerAngles = Vector3.up * 90;
			link.transform.position = new Vector3(node.Col * CUBE_SIZE, 0, node.Row * CUBE_SIZE - LINK_SIZE);
			link.transform.parent = CubeRoot;
		}
		if (node.LinkList[3] != null)
		{
			link = Instantiate(LinkPrefab) as GameObject;
			link.transform.position = new Vector3(node.Col * CUBE_SIZE - LINK_SIZE, 0, node.Row * CUBE_SIZE);
			link.transform.parent = CubeRoot;
		}
	}

	private GameObject CreatePassageGO(PassageType type)
	{
		GameObject prefab = null;
		switch(type)
		{
			case PassageType.One:
				prefab = PassageOne;
				break;
			case PassageType.TwoLine:
				prefab = PassageTwoLine;
				break;
			case PassageType.TwoTurn:
				prefab = PassageTwoTurn;
				break;
			case PassageType.Three:
				prefab = PassageThree;
				break;
			case PassageType.Four:
				prefab = PassageFour;
				break;
		}
		return Instantiate(prefab) as GameObject;
	}
	private GameObject CreateRoomGO(string roomStr)
	{
		GameObject prefab = null;
		switch(roomStr)
		{
			case "MazeBlocks/Room/Room_1":
				prefab = RoomA;
				break;
			case "MazeBlocks/Room/Room_2":
				prefab = RoomB;
				break;
			case "MazeBlocks/Room/Room_3":
				prefab = RoomC;
				break;
		}
		return Instantiate(prefab) as GameObject;
	}
}

