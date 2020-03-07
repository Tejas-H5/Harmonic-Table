using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingsScript : MonoBehaviour {
    public KeyboardScript keyboard;

    public Slider chordSizeSlider;
    public UIBoolToggle touchCircleToggle;
    public UIUpDown rowsUpDown;
    public UIUpDown colsUpDown;
    
    public UIUpDown rootNoteLetter;
    public UIUpDown rootNoteNumber;

    public UIBoolToggle negateUpwards;
    public UIUpDown upwardsInc;

    public UIBoolToggle negateDownwards;
    public UIUpDown downwardsInc;
    public NoteUpdate diagram;

    void OnEnable(){
        chordSizeSlider.value = keyboard.chordSize;

        touchCircleToggle.val = keyboard.touchCircles;
        rowsUpDown.val = keyboard.numRows;
        colsUpDown.val = keyboard.numCols;
        rootNoteLetter.val = Theory.RoundFloat(keyboard.rootNoteLetter);
        rootNoteNumber.val = Theory.RoundFloat(keyboard.rootNoteNumber);
        negateUpwards.val = keyboard.upwardsNegate;
        upwardsInc.val = Theory.RoundFloat(keyboard.upwardsDelta);
        negateDownwards.val = keyboard.downwardsNegate;
        downwardsInc.val = Theory.RoundFloat(keyboard.downwardsDelta);

        diagram.Invalidate();
    }
}
