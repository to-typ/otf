using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler

{
    public Item item;
    public int amount;

    private Image icon;
    private Text amountText;

    private bool isPointerOver;

    void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void SetTextComponent(Text textComponent)
    {
        amountText = textComponent;
    }

    public void SetItem(Item newItem, int count = 1)
    {
        item = newItem;
        amount = count;

        if (item != null)
        {
            icon.sprite = item.icon;
            amountText.text = amount > 1 ? amount.ToString() : "";
        }
        else
            icon.sprite = null;
    }

    public void AddItem(int count = 1)
    {
        amount += count;
        amountText.text = amount.ToString();
    }

    public void Clear()
    {
        item = null;
        amount = 0;
        icon.sprite = null;
    }

    void Update()
    {
        if (isPointerOver && Input.GetKeyDown(KeyCode.X))
        {
            Clear();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
