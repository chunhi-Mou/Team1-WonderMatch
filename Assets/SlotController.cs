using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class SlotController : MonoBehaviour {
    public SlotMachineMove slotMachineMove;
    public List<SlotColumn> columns;   
    public Sprite[] symbols;                 
    public RectTransform[] visibleSlots;     

    private bool isSpinning = false;

    void Start() {
        slotMachineMove.SetToCenter();
        foreach (var col in columns) {
            col.Init(symbols, visibleSlots);
        }
        
    }
    public void StartSlotMachineAnimation() {
        StartSpinning();
        DOVirtual.DelayedCall(0.1f, () => StopWithMatch()).SetUpdate(true);
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
        Sprite match = symbols[idx];
        StartCoroutine(StopColumnsOneByOne(match));
    }

    private System.Collections.IEnumerator StopColumnsOneByOne(Sprite matchSymbol) {
        int count = 0;
        for (int i = 0; i < columns.Count; i++) {
            bool done = false;
            columns[i].StopSpin(matchSymbol, () => done = true);
            yield return new WaitUntil(() => done);
            yield return new WaitForSecondsRealtime(0.3f);
            count++;
        }
        if(count==columns.Count) {
            slotMachineMove.SetToOriginalPos();
        }
    }
}
