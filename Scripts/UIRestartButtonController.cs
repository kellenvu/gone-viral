using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRestartButtonController : UIButtonController {
    protected override void OnClick() {
        TransitionController.instance.Restart();
    }
}
