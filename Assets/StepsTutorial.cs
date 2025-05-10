using UnityEngine;
public class TutorialObj {
    public Card[] card;
    public GameObject[] powers;
    //Undo -Shuffle - Magic - Cell Boost
}
public class StepsTutorial : MonoBehaviour {
    [System.Serializable]
    public static TutorialObj tutorialObj;
}
