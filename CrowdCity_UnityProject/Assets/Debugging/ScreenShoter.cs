using UnityEngine;
using System.Collections;

public class ScreenShoter : MonoBehaviour
{
    // just add this component to any gameObject, change KeyCode in Update() and capture screenShots saving in Assets/ScreenShots
    // IMPORTANT - MAKE SURE YOU USE PROPER SCREEN RESOLUTIONS for iPhone 6, X and iPad
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TakeScreenshot(Application.dataPath + "/../ScreenShots/");
        }
    }
    public void TakeScreenshot(string directory)
    {
        StartCoroutine(TakeScreenshotRoutine(directory));
    }

    public IEnumerator TakeScreenshotRoutine(string directory)
    {
        yield return new WaitForEndOfFrame();
        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, width, height);
        if (MatchesAppleProductResolution(rect) == false)
        {
            Debug.LogWarning("Your resolution setup doesn't match screenshots resolutions required from AppStore");
        }
        tex.ReadPixels(rect, 0, 0);
        tex.Apply();
        var bytes = tex.EncodeToJPG();
        Destroy(tex);
        var filename = string.Format("{0}/screenshot_{1}.jpg", directory, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        if (System.IO.File.Exists(directory) == false)
        {
            System.IO.Directory.CreateDirectory(directory);
        }
        System.IO.File.WriteAllBytes(filename, bytes);
    }

    private bool MatchesAppleProductResolution(Rect rect)
    {
        (float, float)[] resolutions = new (float, float)[] { (1242, 2208), (1284, 2778), (2048, 2732), (1024, 1024), (512, 512)};
        (float, float) currentResolution = (rect.width, rect.height);
        foreach (var resolution in resolutions)
        {
            if (resolution == currentResolution) return true;
        }
        return false;
    }
}

