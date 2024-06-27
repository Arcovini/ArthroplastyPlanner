using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class UIUploadClick : MonoBehaviour
{
    private VisualElement rootVisualElement;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        rootVisualElement = uiDocument.rootVisualElement;

        // Find the Upload-Container VisualElement
        var uploadContainer = rootVisualElement.Q<VisualElement>("Upload-Container");

        // Add a click event listener
        uploadContainer.RegisterCallback<ClickEvent>(OnUploadContainerClicked);
    }

    private void OnUploadContainerClicked(ClickEvent evt)
    {
        Debug.Log("Upload-Container clicked!");

        string path = EditorUtility.OpenFolderPanel("Select Folder", "", "");
        if (!string.IsNullOrEmpty(path))
        {
            Debug.Log("Selected folder: " + path);
            // Handle the folder path
        }
    }
}