using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using Base;
using StaticData;
using Pathfinding;


namespace GameLogic
{
    public enum PathfindingType
    {
        HomeTown,
        Maze,
        Hall,
    }
    public class PathfindingMediator : Mediator
    {
        private PathfindingType currentType = PathfindingType.Maze;
        private Dictionary<PathfindingType, byte[]> graphDataDic = new Dictionary<PathfindingType, byte[]>();

        private BlockProxy blockProxy;

        public override void OnRegister()
        {
            base.OnRegister();
            blockProxy = ApplicationFacade.Instance.RetrieveProxy<BlockProxy>();

            graphDataDic.Add(PathfindingType.HomeTown, null);
            graphDataDic.Add(PathfindingType.Maze, null);
            graphDataDic.Add(PathfindingType.Hall, null);
        }

        public override IList<Enum> ListNotificationInterests ()
        {
            return new Enum[]
            {
                NotificationEnum.PATHFINDING_INIT,
                NotificationEnum.PATHFINDING_DISPOSE,
            };
        }

        public override void HandleNotification (INotification notification)
        {
            switch((NotificationEnum)notification.NotifyEnum)
            {
                case NotificationEnum.PATHFINDING_INIT:
                {
                    PathfindingType type = (PathfindingType)notification.Body;
                    HandlePathfindingInit(type);
                    break;
                }
                case NotificationEnum.PATHFINDING_DISPOSE:
                {
                    bool clearData = (bool)notification.Body;
                    break;
                }
            }
        }

        private void HandlePathfindingInit(PathfindingType type)
        {
            currentType = type;
            switch(type)
            {
                case PathfindingType.HomeTown:
                {
                    break;
                }
                case PathfindingType.Maze:
                {
                    InitMaze();
                    break;
                }
                case PathfindingType.Hall:
                {
                    InitHall();
                    break;
                }
            }
        }
        private void HandlePathfindingDispose(bool clearData)
        {
            AstarPath.active.astarData.DeserializeGraphs(new byte[0]);
            if(clearData)
            {
                graphDataDic[currentType] = null;
            }
        }

        private void InitMaze()
        {
            MazeData mazeData = MazeDataManager.Instance.CurrentMazeData;
            byte[] graphData = graphDataDic[PathfindingType.Maze];
            if(graphData == null)
            {
                blockProxy.ForeachNode((MazeNode node) =>
                    {
                        if(node.Data.BlockType == BlockType.Room)
                        {
                            MazeRoom room = node as MazeRoom;
                            if(!room.HasCreated)
                            {
                                GameObject surface = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, node.Data.GetResPath() + GlobalConfig.PathfindingConfig.WalkSuffix);
                                surface.transform.SetParent(RootTransform.Instance.WalkSurfaceRoot);
                                surface.transform.position = MazeUtil.GetWorldPosition(node.Col, node.Row, mazeData.BlockSize);
                                surface.transform.localEulerAngles = Vector3.up * node.Direction * 90f;
                                room.HasCreated = true;
                            }
                        }
                        else if(node.Data.BlockType == BlockType.Passage)
                        {
                            PassageType type = MazeUtil.GetPassageType(node);
                            GameObject surface = ResourceManager.Instance.LoadGameObject(ObjectType.GameObject, node.Data.GetResPath() + GlobalConfig.PathfindingConfig.WalkSuffix);
                            surface.transform.SetParent(RootTransform.Instance.WalkSurfaceRoot);
                            surface.transform.position = MazeUtil.GetWorldPosition(node.Col, node.Row, mazeData.BlockSize);
                            surface.transform.localEulerAngles = Vector3.up * node.Direction * 90f;
                        }
                    });
                
                RecastGraph graph = AstarPath.active.graphs[0] as RecastGraph;
                graph.useTiles = true;
                graph.editorTileSize = 10;
                graph.cellSize = 0.4f;
                graph.maxSlope = 60f;
                graph.forcedBoundsCenter = new Vector3((mazeData.StartCol * 1f - 0.5f) * mazeData.BlockSize, 0, (mazeData.StartRow * 1f - 0.5f) * mazeData.BlockSize);
                graph.forcedBoundsSize = new Vector3(mazeData.Cols * mazeData.BlockSize, 10, mazeData.Rows * mazeData.BlockSize);
                AstarPath.active.Scan();

                blockProxy.ForeachNode((MazeNode node) =>
                    {
                        if (node.Data.BlockType == BlockType.Room)
                        {
                            MazeRoom room = node as MazeRoom;
                            room.HasCreated = false;
                        }
                    });

                var settings = new Pathfinding.Serialization.SerializeSettings();
                settings.nodes = true;
                graphData = AstarPath.active.astarData.SerializeGraphs(settings);
                graphDataDic[PathfindingType.Maze] = graphData;
            }
            else
            {
                AstarPath.active.astarData.DeserializeGraphs(graphData);
            }
        }
        private void InitHall()
        {
            byte[] graphData = graphDataDic[PathfindingType.Hall];
            if(graphData == null)
            {
                HallData data = Hall.Instance.Data;
                TextAsset ta = Resources.Load<TextAsset>(data.GetResPath() + GlobalConfig.PathfindingConfig.WalkDataSuffix);
                AstarPath.active.astarData.DeserializeGraphs(ta.bytes);
            }
            else
            {
                AstarPath.active.astarData.DeserializeGraphs(graphData);
            }
        }

    }
}

