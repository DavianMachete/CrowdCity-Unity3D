using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManagerSimulation : MonoBehaviour
{
    private int level;


    public void Start()
    {
        DebugManager.instance.Init();
        Init();
    }

    private void Init()
    {
        //Debug.Log("Level is: " + level);
    }

    public void LevelSet(int level)
    {
        this.level = level;
        //Debug.Log("SetLevel : "+ level);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
