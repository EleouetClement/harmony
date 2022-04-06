using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CanvasType
{
    MainMenu,
    PauseMenu,
    DeathMenu,
    GameUI,
    SettingsMenu,
    ExtrasMenu,
    LoadMenu,
    LoadingScreen
}

public class CanvasManager : MonoBehaviour
{
    private static CanvasManager _instance;

    public List<CanvasController> canvasControllerList;
    CanvasController lastActiveCanvas;

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchCanvas(CanvasType.MainMenu);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchCanvas(CanvasType type)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController nextCanvas = canvasControllerList.Find(x => x.canvasType == type);
        if (nextCanvas != null)
        {
            nextCanvas.gameObject.SetActive(true);
            lastActiveCanvas = nextCanvas;
        }
		else
		{
            Debug.LogWarning("Canvas not found!");
		}
    }

    public static CanvasManager GetInstance()
    {
        if (_instance == null)
        {
            Debug.LogError("CanvasManager : not initialized plz fix!!!!!!!!!!!!");
        }
        return _instance;
    }
}
