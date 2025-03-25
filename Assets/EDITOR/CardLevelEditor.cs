using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CardLevelEditor : EditorWindow {
    //DATA
    private CardDatabase cardDatabase;
    private GameObject cardPrefab;

    //LAYER DATA
    private int layerW = 10, layerH = 10;
    private int currLayer = 0;
    private string levelName = "Lv";

    //TAB
    private int selectedTab = 0;
    private string[] tabNames = { "Level Settings", "Cards"};

    //SELECTED DATA
    private bool deleteIsOn = false;
    private Cell selectedCell;
    private CardType defaultCardIdx = CardType.nothing;
    private Vector2 scrollPosition;
    private Dictionary<CardType, int> selectedCardCounts = new Dictionary<CardType, int>();

    [MenuItem("Level Editor/Open Editor")]
    public static void ShowWindow() {
        GetWindow<CardLevelEditor>("Card Level Editor");
    }
    private void OnGUI() {
        selectedTab = GUILayout.Toolbar(selectedTab, tabNames);
        switch (selectedTab) {
            case 0: ShowGridTab(); break;
            case 1: ShowCardSelection(); break;
        }
    }
    private void ShowGridTab() {
        if (!GetData()) return;
        this.ShowGridSettings();
    }
    private bool GetData() {
        EditorGUILayout.LabelField("Select Card Database", EditorStyles.boldLabel);
        cardDatabase = (CardDatabase)EditorGUILayout.ObjectField("Card Database", cardDatabase, typeof(CardDatabase), false);
        EditorGUILayout.LabelField("Card Prefab", EditorStyles.boldLabel);
        cardPrefab = (GameObject)EditorGUILayout.ObjectField("Card Prefab", cardPrefab, typeof(GameObject), false);

        if (cardDatabase == null) {
            EditorGUILayout.HelpBox("Hãy Gắn ScriptableObject: CardDatabase", MessageType.Warning);
            return false;
        }

        if (cardDatabase.cards == null || cardDatabase.cards.Length == 0) {
            EditorGUILayout.HelpBox("No cards in CardDatabase", MessageType.Info);
            return false;
        }
        if (cardPrefab == null) return false;
        return true;
    }
    private void ShowGridSettings() {
        EditorGUILayout.LabelField("Level#", EditorStyles.boldLabel);
        levelName = EditorGUILayout.TextField("Lv#", levelName);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Layer Settings", EditorStyles.boldLabel);
        layerW = EditorGUILayout.IntField("Layer Width", layerW);
        layerH = EditorGUILayout.IntField("Layer Height", layerH);
        currLayer = EditorGUILayout.IntField("Layer#", currLayer);
        if (GUILayout.Button("Create Layer")) BuildLayer();
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
    private void BuildLayer() {
        GameObject parentLevel = GameObject.Find(levelName);
        if (parentLevel == null) {
            parentLevel = new GameObject(levelName);
        }

        GameObject buildGrid = new GameObject("Layer" + currLayer);
        buildGrid.transform.parent = parentLevel.transform;
        BuildLayer layer = buildGrid.AddComponent<BuildLayer>();
        layer.gridSize = new Vector2(layerW, layerH);
        layer.gridCellSize = GetCellSizeFromPrefab();
        layer.BuildCells();
        currLayer++;
        buildGrid.transform.position = new Vector3(0, 0, -currLayer);
        EditorUtility.SetDirty(buildGrid);
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
        obj.GetComponent<CardInfo>().SetCardData(defaultCardIdx);
        EditorUtility.SetDirty(obj);
        return obj;
    }
    private Vector2 GetCellSizeFromPrefab() {
        if (cardPrefab == null) return new Vector2(1, 1);
        Renderer renderer = cardPrefab.GetComponent<Renderer>();
        return renderer != null ? new Vector2(renderer.bounds.size.x, renderer.bounds.size.y) : new Vector2(1, 1);
    }

    private void ShowCardSelection() {
        EditorGUILayout.Space();
        List<GUIContent> contents = GetCardGUIContents();
        if (GUILayout.Button("Done Edit")) ApplyCardSelection();
        if (GUILayout.Button("Cập nhật từ Scene")) {
            UpdateCardSelectionFromScene();
        }


        //Kích thước Card
        float cardWidth = 80;
        float cardHeight = 100;

        float inputHeight = 20;
        float spacing = 10;

        int columns = Mathf.Max(1, Mathf.FloorToInt((position.width - spacing) / (cardWidth + spacing)));

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(400));
        int index = 0;

        for (int i = 0; i < cardDatabase.cards.Length; i += columns) {
            EditorGUILayout.BeginHorizontal();

            for (int j = 0; j < columns && index < cardDatabase.cards.Length; j++, index++) {
                CardData card = cardDatabase.cards[index];
                CardType cardType = card.cardType;

                EditorGUILayout.BeginVertical(GUILayout.Width(cardWidth + spacing));

                GUILayout.FlexibleSpace();

                Texture2D texture = card.sprite ? card.sprite.texture : Texture2D.grayTexture;
                GUILayout.Label(new GUIContent(texture), GUILayout.Width(cardWidth), GUILayout.Height(cardHeight));

                EditorGUILayout.BeginHorizontal();

                // Nút trừ (-3)
                if (GUILayout.Button("-", GUILayout.Width(25), GUILayout.Height(20))) {
                    if (!selectedCardCounts.ContainsKey(cardType)) selectedCardCounts[cardType] = 0;
                    selectedCardCounts[cardType] = Mathf.Max(0, selectedCardCounts[cardType] - 3); //Nhi: Giữ số >= 0
                }

                // Ô nhập số lượng
                if (!selectedCardCounts.ContainsKey(cardType)) selectedCardCounts[cardType] = 0;
                int newValue = EditorGUILayout.IntField(selectedCardCounts[cardType], GUILayout.Width(30), GUILayout.Height(inputHeight));
                selectedCardCounts[cardType] = Mathf.Max(0, Mathf.RoundToInt(newValue / 3.0f) * 3);

                // Nút cộng (+3)
                if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(20))) {
                    if (!selectedCardCounts.ContainsKey(cardType)) selectedCardCounts[cardType] = 0;
                    selectedCardCounts[cardType] += 3;
                }

                EditorGUILayout.EndHorizontal();

                GUILayout.FlexibleSpace();

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }
    private void UpdateCardSelectionFromScene() {
        GameObject parentLevel = GameObject.Find(levelName);
        if (parentLevel == null) {
            EditorUtility.DisplayDialog("Cảnh báo", "Level không tồn tại!", "OK");
            return;
        }
        selectedCardCounts.Clear();

        foreach (Transform layer in parentLevel.transform) {
            foreach (Transform cell in layer) {
                foreach (Transform card in cell) {
                    CardInfo cardComponent = card.GetComponent<CardInfo>();
                    if (cardComponent != null) {
                        CardType type = cardComponent.cardData.cardType;
                        if (!selectedCardCounts.ContainsKey(type)) {
                            selectedCardCounts[type] = 0;
                        }
                        selectedCardCounts[type]++;
                    }
                }
            }
        }
        Repaint();
    }


    private void ApplyCardSelection() {
        GameObject parentLevel = GameObject.Find(levelName);
        if (parentLevel == null) {
            EditorUtility.DisplayDialog("Cảnh báo", "Level không tồn tại!", "OK");
            return;
        }
        List<Transform> allCards = new List<Transform>();
        foreach (Transform layer in parentLevel.transform) {
            foreach (Transform cell in layer) {
                foreach (Transform card in cell) {
                    allCards.Add(card);
                }
            }
        }
        int totalRequiredCards = 0;
        foreach (var kvp in selectedCardCounts) {
            totalRequiredCards += kvp.Value;
        }

        int availableCards = allCards.Count;
        if (availableCards < totalRequiredCards) {
            EditorUtility.DisplayDialog("Cảnh báo", $"Thiếu {totalRequiredCards - availableCards} card để phân bố!", "OK");
        } else if (availableCards > totalRequiredCards) {
            EditorUtility.DisplayDialog("Cảnh báo", $"Có {availableCards - totalRequiredCards} card dư thừa!", "OK");
        }


        Dictionary<CardType, int> remainingCards = new Dictionary<CardType, int>(selectedCardCounts);
        int index = 0;
        foreach (var card in remainingCards) {
            CardType type = card.Key;
            int count = card.Value;
            for (int i = 0; i < count && index < allCards.Count; i++, index++) {
                CardInfo cardComponent = allCards[index].GetComponent<CardInfo>();
                if (cardComponent != null) {
                    cardComponent.SetCardData(type);
                }
            }
        }
        for (; index < allCards.Count; index++) {
            CardInfo cardComponent = allCards[index].GetComponent<CardInfo>();
            if (cardComponent != null) {
                cardComponent.SetCardData(0);
            }
        }
    }
    private List<GUIContent> GetCardGUIContents() {
        List<GUIContent> contents = new List<GUIContent>();

        foreach (CardData card in cardDatabase.cards) {
            Texture2D texture = card.sprite ? card.sprite.texture : Texture2D.grayTexture;
            contents.Add(new GUIContent(texture, card.cardType.ToString()));
        }

        return contents;
    }
}
