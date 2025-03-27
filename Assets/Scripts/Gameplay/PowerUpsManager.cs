using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
public interface IPowerUp {
    void Use();
    void ResetCount(int maxCount);
    int GetCount();
    void SaveData();
    void OnEnable();
    void OnDisable();
}

public class PowerUpsManager : MonoBehaviour {
    public static PowerUpsManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI shuffleCntTxt;
    [SerializeField] private TextMeshProUGUI undoCntTxt;
    [SerializeField] private TextMeshProUGUI magicCntTxt;
    [SerializeField] private TextMeshProUGUI addOneCellCntTxt;
    private StackLogic stack;

    private Dictionary<PowerType, IPowerUp> powerUps = new Dictionary<PowerType, IPowerUp>();
    private int maxPowerCount = 3;

    private void Awake() {
        this.RegisterToGameMode();
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        InitPowerUps();
    }
    private void OnEnable() {
        stack = GameObject.Find("StackA")?.GetComponent<StackLogic>();
        GameEvents.OnCardSelected += CardHistory.Instance.PushCardToHistory;
        powerUps[PowerType.Shuffle].OnEnable();
        powerUps[PowerType.Magic].OnEnable();
    }
    private void OnDisable() {
        GameEvents.OnCardSelected -= CardHistory.Instance.PushCardToHistory;
        powerUps[PowerType.Shuffle].OnDisable();
        powerUps[PowerType.Magic].OnDisable();
    }
    private IEnumerator Start() {
        yield return new WaitForEndOfFrame();
        DOVirtual.DelayedCall(0.15f, () => stack.ShuffleMagicHandler());
    }
    public void RegisterToGameMode() {
        if (SingleModeManager.instance != null) {
            SingleModeManager.instance.SetPowerUpUI(this.gameObject);
        }
    }
    private void InitPowerUps() {
        powerUps[PowerType.Shuffle] = new ShufflePowerUp();
        powerUps[PowerType.Undo] = new UndoPowerUp();
        powerUps[PowerType.Magic] = new MagicPowerUp();
        powerUps[PowerType.AddOneCell] = new AddOneCellPowerUp();
        this.UpdatePowerCountTxtUI();
    }

    public void UseShufflePress() {
        UsePowerUp(PowerType.Shuffle);
    }
    public void UseUndoPress() {
        UsePowerUp(PowerType.Undo);
    }
    public void UseMagicPress() {
        UsePowerUp(PowerType.Magic);
    }
    public void UseAddOneCellPress() {
        UsePowerUp(PowerType.AddOneCell);
    }
    public void UsePowerUp(PowerType type) {
        if (powerUps.ContainsKey(type)) {
            powerUps[type].Use();
            this.UpdatePowerCountTxtUI();
        }
    }
    public void ResetAllPowers() {
        foreach (var power in powerUps.Values) {
            power.ResetCount(maxPowerCount);
        }
    }
    public void ResetPower(PowerType type) {
        powerUps[type].ResetCount(maxPowerCount);
        this.UpdatePowerCountTxtUI();
    }
    public void UpdatePowerCountTxtUI() {
        shuffleCntTxt.text = powerUps[PowerType.Shuffle].GetCount().ToString();
        undoCntTxt.text = powerUps[PowerType.Undo].GetCount().ToString();
        magicCntTxt.text = powerUps[PowerType.Magic].GetCount().ToString();
        addOneCellCntTxt.text = powerUps[PowerType.AddOneCell].GetCount().ToString();
    }
}
