using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteUpdate : MonoBehaviour , IInvalidateable{
    public UIUpDown majorUD;
    public UIBoolToggle majorNegate;
    public UIScrollbarInput majorDistort;
    public UIUpDown minorUD;
    public UIBoolToggle minorNegate;
    public UIScrollbarInput minorDistort;

    public Text fifth;
    public Text major;
    public Text minor;
    public Text half;

    public void Invalidate(){
        float majorInterval = majorUD.val + majorDistort.Value;
        if(majorNegate.val)
            majorInterval = -majorInterval;

        float minorInterval = minorUD.val + minorDistort.Value;
        if(minorNegate.val)
            minorInterval = -minorInterval;
            
        float fifthInterval = majorInterval - minorInterval;
        float halfInterval  = majorInterval + minorInterval;

        fifth.text = Theory.IntervalName(fifthInterval) + "(" + fifthInterval.ToString("+0;-0;") + ")";
        major.text = Theory.IntervalName(majorInterval) + "(" + majorInterval.ToString("+0;-0;") + ")";
        minor.text = Theory.IntervalName(minorInterval) + "(" + minorInterval.ToString("+0;-0;") + ")";
        half.text = Theory.IntervalName(halfInterval) + "(" + halfInterval.ToString("+0;-0;") + ")";
    }
}
