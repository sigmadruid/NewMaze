using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.IO;

using StaticData;
using Pathfinding;

public class PathfindingTool
{
    [MenuItem("Tools/Pathfinding/Scan Hall Graph Data", false, 10)]
    public static void ScanGraphData()
    {
        GameObject walkSurface = Selection.activeGameObject;
        walkSurface.transform.position = GlobalConfig.HallConfig.HallPosition;
        RecastGraph graph = AstarPath.active.graphs[0] as RecastGraph;
        graph.useTiles = true;
        graph.editorTileSize = 10;
        graph.cellSize = 0.2f;
        graph.maxSlope = 60f;
        graph.forcedBoundsCenter = GlobalConfig.HallConfig.HallPosition;
        graph.forcedBoundsSize = new Vector3(300, 100, 300);
        graph.mask = LayerMask.GetMask("LayerWalkSurface");
        AstarPath.active.Scan();
        Debug.Log("scan complete!");
    }

    [MenuItem("Tools/Pathfinding/Generate Hall Graph Data", false, 20)]
    public static void GenerateGraphData()
    {
        GameObject walkSurface = Selection.activeGameObject;
        string name = walkSurface.name + "_WalkData.bytes";
        var settings = new Pathfinding.Serialization.SerializeSettings();
        settings.nodes = true;
        byte[] bytes = AstarPath.active.astarData.SerializeGraphs(settings);
        string path = Application.dataPath + "/Resources/Halls/" + name;
        using(FileStream stream = new FileStream(path, FileMode.Create))
        {
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(bytes);
        }
        AssetDatabase.Refresh();
    }


}

