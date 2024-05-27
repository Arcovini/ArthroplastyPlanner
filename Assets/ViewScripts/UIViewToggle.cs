using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIViewToggle : VisualElement
{
    public new class UxmlFactory : UxmlFactory<UIViewToggle, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription promptAttr = new UxmlStringAttributeDescription()
        {
            name = "promptTexture",
        };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var viewToggle = ve as UIViewToggle;
            string promptPath = promptAttr.GetValueFromBag(bag, cc);
            viewToggle.SetPromptTexture(promptPath);
        }
    }

    private string promptTexturePath;
    private Texture2D promptTexture;

    public string PromptTexturePath
    {
        get => promptTexturePath;
        private set
        {
            promptTexturePath = value;
            LoadPromptTexture();
            UpdatePrompt();
        }
    }

    private Button button_container;

    private const string styleResource = "StyleSheetViewToggle";
    private const string uss_viewtoggle_container = "viewtoggle_container";
    private const string uss_viewtoggle_bgimg = "viewtoggle_bgimg";

    private bool isChecked;

    public UIViewToggle()
    {
        styleSheets.Add(Resources.Load<StyleSheet>(styleResource));
        AddToClassList(uss_viewtoggle_container);

        button_container = new Button();
        hierarchy.Add(button_container);
        button_container.AddToClassList(uss_viewtoggle_bgimg);
        button_container.clicked += OnToggleClicked;

        style.backgroundColor = new StyleColor(new Color(0xD9 / 255f, 0xD9 / 255f, 0xD9 / 255f));
    }

    private void OnToggleClicked()
    {
        isChecked = !isChecked;
        UpdateBackgroundColor();
    }

    private void UpdateBackgroundColor()
    {
        if (isChecked)
        {
            style.backgroundColor = new StyleColor(new Color(0x87 / 255f, 0xE4 / 255f, 0xE4 / 255f)); // #87E4E4
        }
        else
        {
            style.backgroundColor = new StyleColor(new Color(0xD9 / 255f, 0xD9 / 255f, 0xD9 / 255f)); // #D9D9D9
        }
    }

    public void SetPromptTexture(string promptTexturePath)
    {
        if (!string.IsNullOrEmpty(promptTexturePath))
        {
            PromptTexturePath = promptTexturePath;
        }
    }

    private void LoadPromptTexture()
    {
        if (!string.IsNullOrEmpty(promptTexturePath))
        {
            promptTexture = Resources.Load<Texture2D>(promptTexturePath);
        }
    }

    private void UpdatePrompt()
    {
        if (promptTexture != null)
        {
            button_container.style.backgroundImage = new StyleBackground(promptTexture);
        }
        else
        {
            button_container.style.backgroundImage = new StyleBackground();
        }
    }
}
