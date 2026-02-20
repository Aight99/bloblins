#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelConfig levelConfig = (LevelConfig)target;

        EditorGUILayout.LabelField("Level Configuration", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Map Layout Legend:\n"
                + "G - Ground (walkable)\n"
                + "W - Water (not walkable)\n"
                + "_ - Void (not walkable)\n\n"
                + "Preview Legend:\n"
                + "B - Bloblin (black)\n"
                + "E - Enemy (red)\n"
                + "O - Item (black)\n\n"
                + "Map dimensions are calculated automatically based on the layout.",
            MessageType.Info
        );

        if (GUILayout.Button("Preview Map"))
        {
            PreviewMap(levelConfig);
        }
    }

    private void PreviewMap(LevelConfig levelConfig)
    {
        if (string.IsNullOrEmpty(levelConfig.MapLayout))
        {
            EditorUtility.DisplayDialog("Preview Error", "Map layout is empty!", "OK");
            return;
        }

        string[] rows = levelConfig.MapLayout.Split('\n');
        int height = rows.Length;
        int width = 0;

        foreach (string row in rows)
        {
            if (row.Length > width)
                width = row.Length;
        }

        EditorWindow window = EditorWindow.GetWindow(typeof(LevelPreviewWindow));
        window.titleContent = new GUIContent("Level Preview");
        window.minSize = new Vector2(width * 20 + 40, height * 20 + 60);

        if (window is LevelPreviewWindow previewWindow)
        {
            previewWindow.SetLevelConfig(levelConfig);
        }
    }
}

public class LevelPreviewWindow : EditorWindow
{
    private LevelConfig levelConfig;
    private GUIStyle cellStyle;
    private GUIStyle bloblinStyle;
    private GUIStyle enemyStyle;
    private GUIStyle itemStyle;
    private GUIStyle labelStyle;

    public void SetLevelConfig(LevelConfig config)
    {
        levelConfig = config;
    }

    private void OnEnable()
    {
        cellStyle = new GUIStyle();
        cellStyle.normal.background = EditorGUIUtility.whiteTexture;
        cellStyle.alignment = TextAnchor.MiddleCenter;

        bloblinStyle = new GUIStyle();
        bloblinStyle.normal.textColor = Color.black;
        bloblinStyle.alignment = TextAnchor.MiddleCenter;
        bloblinStyle.fontSize = 12;
        bloblinStyle.fontStyle = FontStyle.Bold;

        enemyStyle = new GUIStyle();
        enemyStyle.normal.textColor = Color.red;
        enemyStyle.alignment = TextAnchor.MiddleCenter;
        enemyStyle.fontSize = 12;
        enemyStyle.fontStyle = FontStyle.Bold;

        itemStyle = new GUIStyle();
        itemStyle.normal.textColor = Color.black;
        itemStyle.alignment = TextAnchor.MiddleCenter;
        itemStyle.fontSize = 10;
        itemStyle.fontStyle = FontStyle.Bold;

        labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.white;
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 8;
    }

    private void OnGUI()
    {
        if (levelConfig == null)
            return;

        string[] rows = levelConfig.MapLayout.Split('\n');
        int height = rows.Length;

        EditorGUILayout.LabelField("Map Preview", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        float cellSize = 20;
        float startX = 20;
        float startY = 40;

        for (int y = 0; y < height; y++)
        {
            string row = rows[y].TrimEnd();
            for (int x = 0; x < row.Length; x++)
            {
                Rect cellRect = new Rect(
                    startX + x * cellSize,
                    startY + y * cellSize,
                    cellSize,
                    cellSize
                );

                CellType cellType = levelConfig.GetCellTypeAt(x, y);
                Color cellColor = GetCellColor(cellType);
                GUI.backgroundColor = cellColor;
                GUI.Box(cellRect, "", cellStyle);

                GUI.Label(cellRect, $"{x},{y}", labelStyle);
            }
        }

        foreach (var bloblin in levelConfig.Bloblins)
        {
            Rect bloblinRect = new Rect(
                startX + bloblin.X * cellSize,
                startY + bloblin.Y * cellSize,
                cellSize,
                cellSize
            );
            GUI.Label(bloblinRect, "B", bloblinStyle);
        }

        foreach (var enemy in levelConfig.Enemies)
        {
            Rect enemyRect = new Rect(
                startX + enemy.X * cellSize,
                startY + enemy.Y * cellSize,
                cellSize,
                cellSize
            );
            GUI.Label(enemyRect, "E", enemyStyle);
        }

        foreach (var item in levelConfig.Items)
        {
            Rect itemRect = new Rect(
                startX + item.X * cellSize,
                startY + item.Y * cellSize,
                cellSize,
                cellSize
            );
            GUI.Label(itemRect, "O", itemStyle);
        }
    }

    private Color GetCellColor(CellType cellType)
    {
        switch (cellType)
        {
            case CellType.Ground:
                return new Color(0.267f, 0.792f, 0.565f);
            case CellType.Water:
                return new Color(0.2f, 0.2f, 0.8f);
            case CellType.Void:
                return new Color(0.1f, 0.1f, 0.1f);
        }
        throw new System.NotImplementedException();
    }
}
#endif
