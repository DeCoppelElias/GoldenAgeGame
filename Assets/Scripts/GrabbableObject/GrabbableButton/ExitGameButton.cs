using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButton : AllGrabbableButton
{
    protected override void ButtonEffect()
    {
        Application.Quit();
    }
}
