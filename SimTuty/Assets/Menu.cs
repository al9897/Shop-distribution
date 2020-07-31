using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string SimulationSceneName;
    public string MapBuildingSceneName; 

    public void LoadSimulationScene()
    {
        SceneManager.LoadScene(SimulationSceneName);
    }

    public void LoadConfigScene()
    {
        SceneManager.LoadScene(MapBuildingSceneName);
    }

}
