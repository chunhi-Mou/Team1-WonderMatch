using UnityEngine;

[System.Serializable]
public class TutorialObj {
    public Card[] card;
    public GameObject[] powers;
    // Undo - Shuffle - Magic - Cell Boost
}

public class StepsTutorial : MonoBehaviour {
    public TutorialObj tutorialObj;
}
