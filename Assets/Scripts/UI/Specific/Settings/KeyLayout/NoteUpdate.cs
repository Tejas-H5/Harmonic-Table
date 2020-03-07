using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteUpdate : MonoBehaviour , IInvalidateable{
    public UIUpDown majorUD;
    public UIBoolToggle majorNegate;
    public Slider majorDistort;
    public UIUpDown minorUD;
    public UIBoolToggle minorNegate;
    public Slider minorDistort;

    public Text fifth;
    public Text major;
    public Text minor;
    public Text half;

    public void Invalidate(){
        float majorInterval = majorUD.val + majorDistort.value;
        if(majorNegate.val)
            majorInterval = -majorInterval;

        float minorInterval = minorUD.val + minorDistort.value;
        if(minorNegate.val)
            minorInterval = -minorInterval;
            
        float fifthInterval = majorInterval - minorInterval;
        float halfInterval  = majorInterval + minorInterval;

        fifth.text = Theory.IntervalName(fifthInterval) + "\n" + Theory.Distortion(fifthInterval);
        major.text = Theory.IntervalName(majorInterval) + "\n" + Theory.Distortion(majorInterval);
        minor.text = Theory.IntervalName(minorInterval) + "\n" + Theory.Distortion(minorInterval);
        half.text = Theory.IntervalName(halfInterval) + "\n" + Theory.Distortion(halfInterval);
    }
}
