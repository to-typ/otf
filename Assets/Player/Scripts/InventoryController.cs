using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Hotbar Settings")]
    public RectTransform invTransform;
    public Vector2 invSize = new Vector2(8, 8);
    public Vector2 slotSize = new Vector2(80, 80);
    public float slotSpacing = 10f;

    private GameObject[][] slots;
    private Item[][] items;
    public static bool isInvOpen = false;

    private SlotBehavior[][] slotScripts;

    void Start()
    {
        float startX = slotSpacing;
        float startY = slotSpacing;

        slots = new GameObject[(int)invSize.x][];
        slotScripts = new SlotBehavior[(int)invSize.x][];
        for (int i = 0; i < (int)invSize.x; i++)
        {
            slots[i] = new GameObject[(int)invSize.y];
            slotScripts[i] = new SlotBehavior[(int)invSize.y];

            for (int j = 0; j < (int)invSize.y; j++)
            {
                // Neues GameObject f¸r Slot
                GameObject slot = new GameObject($"Slot_{i + 1}{j + 1}", typeof(RectTransform), typeof(Image), typeof(Button), typeof(SlotBehavior));

                slot.transform.SetParent(invTransform, false);

                RectTransform rt = slot.GetComponent<RectTransform>();
                rt.sizeDelta = slotSize;

                // Position berechnen
                float posX = startX + i * (slotSize.x + slotSpacing);
                float posY = startY + j * (slotSize.x + slotSpacing);

                // Position setzen (Anchor links unten)
                rt.anchorMin = new Vector2(0, 0);
                rt.anchorMax = new Vector2(0, 0);
                rt.pivot = new Vector2(0, 0);
                rt.anchoredPosition = new Vector2(posX, posY);

                // Image auf weiﬂ setzen
                Image img = slot.GetComponent<Image>();
                img.color = Color.white;

                // Text als separates Child-GameObject
                GameObject textObj = new GameObject("ItemCount", typeof(RectTransform), typeof(Text));
                textObj.transform.SetParent(slot.transform, false);

                // Text-RectTransform konfigurieren
                RectTransform textRT = textObj.GetComponent<RectTransform>();
                textRT.anchorMin = Vector2.zero;
                textRT.anchorMax = Vector2.one;
                textRT.offsetMin = Vector2.zero;
                textRT.offsetMax = Vector2.zero;

                // Text-Komponente konfigurieren
                Text textComponent = textObj.GetComponent<Text>();
                textComponent.text = "";
                textComponent.alignment = TextAnchor.LowerRight;
                textComponent.fontSize = 71;
                textComponent.color = Color.red;
                textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                textComponent.raycastTarget = false;

                slots[i][j] = slot;
                slotScripts[i][j] = slot.GetComponent<SlotBehavior>();
                slotScripts[i][j].SetTextComponent(textComponent);
                slotScripts[i][j].Clear();
            }
        }
        invTransform.gameObject.SetActive(isInvOpen);
        demo();

    }  

    void demo() {
        pickupItem(Resources.Load<Item>("Items/Wood"));
        pickupItem(Resources.Load<Item>("Items/Stone"));
        pickupItem(Resources.Load<Item>("Items/Stone"));
        pickupItem(Resources.Load<Item>("Items/Metal"));
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            isInvOpen = !isInvOpen;
            if (isInvOpen)
            {
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;
            }
            else
            {
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
        }
        invTransform.gameObject.SetActive(isInvOpen);
    }

    void pickupItem(Item item, int count = 1)
    {
        for (int i = 0; i < (int)invSize.x; i++)
        {
            for (int j = 0; j < (int)invSize.y; j++)
            {
                SlotBehavior slot = slotScripts[i][j];

                if (slot.item == null)
                {
                    slot.SetItem(item, count);
                    return;
                }
                else if (slot.item == item)
                {
                    slot.AddItem(count);
                    return;
                }
            }
        }
    }
}
