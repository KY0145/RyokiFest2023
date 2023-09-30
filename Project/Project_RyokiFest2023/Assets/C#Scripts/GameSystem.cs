using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
