using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Implementation of GameMenu. 

public class PauseMenu : GameMenu {

    public static PauseMenu instance;

    protected override void Awake() {
        if (instance == null) 
            instance = this;
        base.Awake();
    }


}