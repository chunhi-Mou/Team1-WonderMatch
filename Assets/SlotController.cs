using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour {
    public SlotMachineMove slotMachineMove;
    public List<SlotColumn> columns;
    public Sprite[] symbols;
    public RectTransform[] visibleSlots;

    [Header("Light")]
    public Image[] lights;
    public Sprite lightOn;
    public Sprite lightOff;
    private List<Tween> blinkTweens = new List<Tween>();

    private bool isSpinning = false;
    public static int IdxCardType;

    void Start() {
        slotMachineMove.SetToCenter();
        foreach (var col in columns) {
            col.Init(symbols, visibleSlots);
        }
    }

    public void StartSlotMachineAnimation() {
        AudioManager.instance.Play(SoundEffect.SlotMachine);
        StartSpinning();
        StartBlinking(0.75f, 0.25f);
        DOVirtual.DelayedCall(0.05f, () => StopWithMatch()).SetUpdate(true).SetAutoKill(true);
    }

    public void StartSpinning() {
        isSpinning = true;
        foreach (var col in columns) {
            col.StartSpin();
        }
    }

    public void StopWithMatch() {
        if (!isSpinning) return;
        isSpinning = false;

        int idx = Random.Range(0, symbols.Length);
        IdxCardType = idx;
        Sprite match = symbols[idx];
        StartCoroutine(StopColumnsOneByOne(match));
    }

    private System.Collections.IEnumerator StopColumnsOneByOne(Sprite matchSymbol) {
        int count = 0;
        for (int i = 0; i < columns.Count; i++) {
            bool done = false;
            columns[i].StopSpin(matchSymbol, () => done = true);
            yield return new WaitUntil(() => done);
            yield return new WaitForSecondsRealtime(0.1f);
            AudioManager.instance.PlayOneShot(SoundEffect.SlotChosen);
            count++;
        }
        if (count == columns.Count) {
            slotMachineMove.SetToOriginalPos();
            GameEvents.OnDoneChooseCardTypeInvoke();
            StopBlinking();
            SetLightOn();
        }
    }

    public void StartBlinking(float blinkDuration, float blinkInterval) {
        StopBlinking();

        foreach (var light in lights) {
            float randomDelay = UnityEngine.Random.Range(0f, blinkDuration);
            float randomInterval = UnityEngine.Random.Range(blinkInterval * 0.5f, blinkInterval * 1.5f);

            Tween lightTween = DOTween.To(() => 0f, x => {
                light.sprite = (x > 0.5f) ? lightOn : lightOff;
                light.SetNativeSize();
            }, 1f, randomInterval)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(randomDelay)
            .SetUpdate(true).SetAutoKill(true);

            blinkTweens.Add(lightTween);
        }
    }

    public void SetLightOn() {
        foreach (var light in lights) {
            light.sprite = lightOn;
            light.SetNativeSize();
        }
    }

    public void StopBlinking() {
        foreach (var tween in blinkTweens) {
            tween.Kill();
        }
        blinkTweens.Clear();
    }
}
