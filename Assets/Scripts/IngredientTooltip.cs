using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private Text nameText;
    [SerializeField] private Text effectsText;

    private Ingredient ingredient;
    private CloneDragAndDrop cloneDragAndDrop;

    private void Awake()
    {
        ingredient = GetComponent<Ingredient>();
        cloneDragAndDrop = GetComponent<CloneDragAndDrop>();
        tooltipObject.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cloneDragAndDrop.isOriginalInstance)
        {
            UpdateTooltipText();
            ShowTooltip();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void ShowTooltip()
    {
        tooltipObject.SetActive(true);
        tooltipObject.transform.SetAsLastSibling();
    }

    private void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }

    private void UpdateTooltipText()
    {
        nameText.text = ingredient.Name;
        effectsText.text = string.Join(", ", ingredient.Effects);
    }
}