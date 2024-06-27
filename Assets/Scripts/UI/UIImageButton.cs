using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIImageButton : Button
{
    public new class UxmlFactory : UxmlFactory<UIImageButton, UxmlTraits> { }

    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription promptAttr = new UxmlStringAttributeDescription()
        {
            name = "promptTexture",
        };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var imageButton = ve as UIImageButton;
            string promptPath = promptAttr.GetValueFromBag(bag, cc);
            imageButton.SetPromptTexture(promptPath);
        }
    }

    private const string styleResource = "Styles/StyleSheetImgButton";
    private const string uss_imgbutton_container = "imagebutton_button";
    private const string uss_imgbutton_bgimg = "imagebutton_img";

    private VisualElement bgImage;

    public UIImageButton()
    {
        styleSheets.Add(Resources.Load<StyleSheet>(styleResource));
        AddToClassList(uss_imgbutton_container);

        bgImage = new VisualElement();
        hierarchy.Add(bgImage);
        bgImage.AddToClassList(uss_imgbutton_bgimg);

    }

    public void SetPromptTexture(string texturePath)
    {
        var texture = Resources.Load<Texture2D>(texturePath);
        if (texture != null)
        {
            bgImage.style.backgroundImage = new StyleBackground(texture);
        }
        else
        {
            Debug.LogWarning($"Texture not found at path: {texturePath}");
        }
    }
}