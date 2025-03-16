using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelEditor : EditorWindow {
    private CardDatabase cardDatabase;
    private GameObject cardPrefab;

    private int gridW = 10, gridH = 10;
    private int currLayer = 0;
    private string levelName;

    private Cell selectedCell;
    private bool deleteIsOn = false;
    private int selectedCardIdx = 0;
    private Vector2 scrollPosition;

    private int selectedTab = 0;
    private string[] tabNames = { "Grid", "Cards In Use", "Cards Missing" };
    [MenuItem("Level Builder/Open Builder")]
    public static void ShowWindow() {
        GetWindow<LevelEditor>("Level Editor");
    }
    private void OnGUI() {
        selectedTab = GUILayout.Toolbar(selectedTab, tabNames);
        if (selectedTab == 0) {
            ShowGridTab();
        }
        if (selectedTab == 1) {
            ShowCardsInUse();
        }
        if (selectedTab == 2) {
            ShowCardsMissing();
        }
    }
    #region Draw Grid Tab
    private void ShowGridTab() {
        if (!ShowUpDataPart()) return;
        ShowGridSettings();
        ShowCardSelection();
    }
    private void ShowGridSettings() {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Settings", EditorStyles.boldLabel);

        levelName = EditorGUILayout.TextField("Level Name", levelName);
        gridW = EditorGUILayout.IntField("Grid Width", gridW);
        gridH = EditorGUILayout.IntField("Grid Height", gridH);
        currLayer = EditorGUILayout.IntField("Current Layer", currLayer);
        deleteIsOn = EditorGUILayout.Toggle("Delete Mode", deleteIsOn);
    }

    private void ShowCardSelection() {
        EditorGUILayout.Space();
        List<GUIContent> contents = GetCardGUIContents();

        float cardWidth = 100;
        float cardHeight = 150;
        int columns = Mathf.Max(1, Mathf.FloorToInt(position.width / cardWidth));

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));
        selectedCardIdx = GUILayout.SelectionGrid(selectedCardIdx, contents.ToArray(), columns,
                            GUILayout.Width(columns * cardWidth),
                            GUILayout.Height((cardDatabase.cards.Length / columns + 1) * cardHeight));
        EditorGUILayout.EndScrollView();
    }

    private List<GUIContent> GetCardGUIContents() {
        List<GUIContent> contents = new List<GUIContent>();

        foreach (CardData card in cardDatabase.cards) {
            Texture2D texture = card.sprite ? card.sprite.texture : Texture2D.grayTexture;
            contents.Add(new GUIContent(texture, card.cardType.ToString()));
        }

        return contents;
    }

    private bool ShowUpDataPart() {
        EditorGUILayout.LabelField("Select Card Database", EditorStyles.boldLabel);
        cardDatabase = (CardDatabase)EditorGUILayout.ObjectField("Card Database", cardDatabase, typeof(CardDatabase), false);
        EditorGUILayout.LabelField("Card Prefab", EditorStyles.boldLabel);
        cardPrefab = (GameObject)EditorGUILayout.ObjectField("Card Prefab", cardPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Create Grid")) BuildGrid();

        if (cardDatabase == null) {
            EditorGUILayout.HelpBox("Hãy Gắn ScriptableObject: CardDatabase", MessageType.Warning);
            return false;
        }

        if (cardDatabase.cards == null || cardDatabase.cards.Length == 0) {
            EditorGUILayout.HelpBox("No cards in CardDatabase", MessageType.Info);
            return false;
        }
        return true;
    }
    #endregion
    #region Draw Cards Tab
    private void ShowCardsInUse() {
        Dictionary<string, int> cardCount = CountCardsByType();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Cards In Use", EditorStyles.boldLabel);

        if (cardCount.Count == 0) {
            EditorGUILayout.HelpBox("No cards placed", MessageType.Info);
            return;
        }

        foreach (var entry in cardCount) {
            EditorGUILayout.LabelField($"{entry.Key}: {entry.Value}");
        }
    }
    private void ShowCardsMissing() {
        Dictionary<string, int> cardCount = CountCardsByType();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Cards Missing", EditorStyles.boldLabel);

        bool hasMissing = false;
        foreach (var cardType in cardCount) {
            int missing = (3 - (cardType.Value % 3)) % 3;
            if (missing > 0) {
                hasMissing = true;
                EditorGUILayout.LabelField($"{cardType.Key}: Needs {missing} more");
            }
        }

        if (!hasMissing) {
            EditorGUILayout.HelpBox("VALID", MessageType.Info);
        }
    }

    private Dictionary<string, int> CountCardsByType() {
        Dictionary<string, int> cardCount = new Dictionary<string, int>();

        GameObject parentLevel = GameObject.Find(levelName);
        if (parentLevel == null) return cardCount;

        foreach (Transform layer in parentLevel.transform) {
            foreach (Transform cell in layer) {
                foreach (Transform card in cell) {
                    string type = card.GetComponent<Card>().cardData.cardType.ToString();
                    if (!cardCount.ContainsKey(type)) cardCount[type] = 0;
                    cardCount[type]++;
                }
            }
        }
        return cardCount;
    }

    #endregion
    private void BuildGrid() {
        GameObject parentLevel = GameObject.Find(levelName);
        if (parentLevel == null) {
            parentLevel = new GameObject(levelName);
        }

        GameObject buildGrid = new GameObject("Layer" + currLayer);
        buildGrid.transform.parent = parentLevel.transform;
        BuildLayer layer = buildGrid.AddComponent<BuildLayer>();
        layer.gridSize = new Vector2(gridW, gridH);
        layer.gridCellSize = GetCellSizeFromPrefab();
        layer.BuildCells();
        currLayer++;
        buildGrid.transform.position = new Vector3(0, 0, -currLayer);

        EditorUtility.SetDirty(buildGrid);
    }

    private void OnSelectionChange() {
        if (Selection.activeGameObject != null) {
            Cell area = Selection.activeGameObject.GetComponent<Cell>();
            if (area != null) {
                selectedCell = area;
                if (deleteIsOn) {
                    ClearOldObjects(selectedCell);
                } else {
                    PlaceNewObject();
                }
            } else {
                selectedCell = null;
            }
        }
    }

    private void ClearOldObjects(Cell cell) {
        for (int i = cell.transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(cell.transform.GetChild(i).gameObject);
        }
        EditorUtility.SetDirty(cell);
    }

    private GameObject PlaceNewObject() {
        if (selectedCell == null || cardPrefab == null) return null;

        ClearOldObjects(selectedCell);
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(cardPrefab, selectedCell.transform);
        PrefabUtility.UnpackPrefabInstance(obj, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

        obj.transform.position = selectedCell.transform.position;
        obj.transform.rotation = cardPrefab.transform.rotation;
        obj.transform.localScale = cardPrefab.transform.localScale;
        obj.GetComponent<Card>().SetCardData(selectedCardIdx);
        EditorUtility.SetDirty(obj);
        return obj;
    }

    private Vector2 GetCellSizeFromPrefab() {
        if (cardPrefab == null) return new Vector2(1, 1);
        Renderer renderer = cardPrefab.GetComponent<Renderer>();
        return renderer != null ? new Vector2(renderer.bounds.size.x, renderer.bounds.size.y) : new Vector2(1, 1);
    }
}
