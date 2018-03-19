using UnityEngine;
using System.Collections;

public enum DemoSceneTypes {
	MainMenu,
	TowerDefenseDemo,
	LaserBeamsDemo
}

public class LBWSP_DemoGUI : MonoBehaviour {

	public DemoSceneTypes DemoSceneType = DemoSceneTypes.MainMenu;

	// Use this for initialization
	void Start () {
	
	}

	public string packageNameLabel = "Laser Beams";
	public string versionLabel = " v1";
	public string packageCreatorLabel = "for catsert";
	
	void OnGUI () {
		GUI.Label(new Rect(10, 10, 250, 50), packageNameLabel + versionLabel + "\n" + packageCreatorLabel);
		if (DemoSceneType == DemoSceneTypes.TowerDefenseDemo) {
			GUI.Label(new Rect(10, 60, 250, 20), DemoGUIManager.GlobalAccess.TurretLabel);
			GUI.Label(new Rect(10, 70, 250, 20), DemoGUIManager.GlobalAccess.TurretDamageLabel);
		}

        if (DemoSceneType == DemoSceneTypes.MainMenu)
        {
            if (GUI.Button(new Rect(10, 90, 180, 30), "Laser Beams Demo"))
            {
                // Load Laser Beams Demo Scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
            if (GUI.Button(new Rect(10, 120, 180, 30), "Tower Defense Demo"))
            {
                // Load Tower Defense Demo Scene
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
        }

            if (GUI.Button(new Rect(10, 30, 180, 30), "Change Camera"))
            {
                // Change to Next Camera
                WSP_DemoController.GlobalAccess.ChangeCamera();
            }
          
    }

	// Update is called once per frame
	void Update () {
	
	}
}
