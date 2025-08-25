using UnityEngine;
using UnityEngine.UI;

public class HotbarGenerator : MonoBehaviour
{
    [Header("Hotbar Settings")]
    public RectTransform hotbarTransform;   // Das Panel mit Größe 1000x100
    public int slotCount = 10;
    public Vector2 slotSize = new Vector2(80, 80);
    public float slotSpacing = 10f;

    private GameObject[] slots;
    private int selectedSlot = 1;
    private KeyCode[] keyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
    };

    void Start()
    {
        GenerateHotbarSlots();
    }

    void GenerateHotbarSlots()
    {
        slots = new GameObject[slotCount];

        // Breite und Höhe der Hotbar
        float hotbarWidth = hotbarTransform.rect.width;
        float hotbarHeight = hotbarTransform.rect.height;

        // Erste Slot-Position (Start bei 10px Abstand vom linken Rand, vertikal mittig)
        float startX = slotSpacing;
        float posY = -(hotbarHeight / 2 - slotSize.y / 2 - slotSpacing);

        for (int i = 0; i < slotCount; i++)
        {
            // Neues GameObject für Slot
            GameObject slot = new GameObject($"Slot_{i + 1}", typeof(RectTransform), typeof(Image));
            slot.transform.SetParent(hotbarTransform, false);

            RectTransform rt = slot.GetComponent<RectTransform>();
            rt.sizeDelta = slotSize;

            // X-Position berechnen
            float posX = startX + i * (slotSize.x + slotSpacing);

            // Position setzen (Anchor links unten)
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(0, 0);
            rt.pivot = new Vector2(0, 0);
            rt.anchoredPosition = new Vector2(posX, slotSpacing);

            // Image auf weiß setzen
            Image img = slot.GetComponent<Image>();
            img.color = Color.white;

            slots[i] = slot;
        }
        slots[0].GetComponent<Image>().color = Color.gray;
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                slots[selectedSlot].GetComponent<Image>().color = Color.white;
                selectedSlot = i;
                slots[i].GetComponent<Image>().color = Color.gray;
            }

        }
    }
}
