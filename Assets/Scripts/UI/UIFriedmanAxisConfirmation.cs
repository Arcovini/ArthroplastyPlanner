using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    private VisualElement rootVisualElement;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        rootVisualElement = uiDocument.rootVisualElement;

        // Find the button VisualElement (assuming the button has the name "ChangeSceneButton")
        var changeSceneButton = rootVisualElement.Q<Button>("FriedMannAxisConfirmationButton");

        // Add a click event listener
        changeSceneButton.clicked += OnChangeSceneButtonClicked;

        // Register the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unregister the scene loaded event when the object is disabled
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnChangeSceneButtonClicked()
    {
        Debug.Log("ChangeSceneButton clicked!");

        // Try loading the scene asynchronously and catch any potential exceptions
        try
        {
            string sceneName = "Planner"; // Ensure this is the exact name of your scene
            Debug.Log($"Attempting to load scene: {sceneName}");
            StartCoroutine(LoadSceneAsync(sceneName));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load scene: {ex.Message}");
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            Debug.Log($"Loading scene {sceneName}: {asyncLoad.progress * 100}%");
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} loaded successfully!");
    }
}