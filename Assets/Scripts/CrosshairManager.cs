using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] [Range(0, 1)] private float opacity = 0.3f;
    [SerializeField] private Sprite[] crosshairSprites;

    [Header("Info - No Touch")]
    public Image crosshairImage;
    public int currentCrosshairSpriteIndex;

    private PlayerStateData psd;
    private PlayerAimManager pam;

    private Color temporaryColor;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pam = GetComponent<PlayerAimManager>();
        crosshairImage = GetComponentInChildren<Image>();

        //Default values
        temporaryColor = Color.white;
        temporaryColor.a = opacity;
        crosshairImage.color = temporaryColor;
        ChangeCrosshairImage(0);
    }

    private void Update()
    {
        HandleCrosshairColor();
    }

    private void HandleCrosshairColor()
    {
        //TODO: DOESN'T DETECT DEAD ENEMIES

        //We can not directly change crosshairImage.color.a
        //We can only assign a color variable to it. Therefore we need a temporary color variable..
        //..to make changes upon and finally assign it

        temporaryColor = Color.white;

        if (pam.canInteract)
        {
            temporaryColor.a = 1f;
        }

        else if (pam.canAttack && psd.isAiming)
        {
            temporaryColor = Color.red;
            temporaryColor.a = 1f;
        }

        else temporaryColor.a = opacity;

        crosshairImage.color = temporaryColor;
    }

    public void ChangeCrosshairImage(int crosshairSpriteIndex)
    {
        crosshairImage.sprite = crosshairSprites[crosshairSpriteIndex];
        currentCrosshairSpriteIndex = crosshairSpriteIndex;

        if (crosshairSpriteIndex == 0) crosshairImage.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        else crosshairImage.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }
}