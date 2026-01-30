using System;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSelect : MonoBehaviour
{
    private ToggleGroup tg;

    void Start()
     {
        tg = GetComponent<ToggleGroup>();
        UIManager.UIM.registerWorldUILvlSelectScript(this);
     }

    public int getLevelSelected()
    {
        if (tg == null) return 0;

        Toggle tgSelected = tg.GetFirstActiveToggle();

        if (tgSelected == null) return 0;

        int lvlSelected = 0;

        switch (tgSelected.name)
        {
            case "Lvl1":
                lvlSelected = 1;
                break;

            case "Lvl2":
                lvlSelected = 2;
                break;

            case "Lvl3":
                lvlSelected = 3;
                break;
        }

        return lvlSelected;

    }
    
    public void unlockLevel2()
    {
        foreach (Toggle t in tg.GetComponentsInChildren<Toggle>(true))
        {
            if (t.name.Equals("Lvl2"))
            {
                t.interactable = true;
            }
        }
    }
}
