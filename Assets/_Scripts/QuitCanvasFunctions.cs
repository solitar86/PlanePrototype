using UnityEditor;
using UnityEngine;

public class QuitCanvasFunctions : MonoBehaviour
{

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

    }
}