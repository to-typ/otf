using UnityEngine;
using UnityEngine.UI;

public class EscapeController : MonoBehaviour
{
    [Header("Escape Menu Settings")]
    public RectTransform escTransform;
    public Vector2 buttonSize = new Vector2(200, 60);
    public float buttonSpacing = 15f;

    // Texte für die Buttons (kannst du im Inspector auch ändern)
    public string[] buttonLabels = { "Weiter", "Einstellungen", "Hauptmenü" };

    private GameObject[] buttons;
    private bool buttonsCreated = false;
    public static bool isEscOpen = false;

    void Start()
    {
        escTransform.gameObject.SetActive(false); // Menü beim Start unsichtbar
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEscOpen = !isEscOpen;
            escTransform.gameObject.SetActive(isEscOpen);

            if (isEscOpen)
            {
                Time.timeScale = 0;

                UnityEngine.Cursor.lockState = CursorLockMode.None;
                UnityEngine.Cursor.visible = true;

                if (!buttonsCreated)
                {
                    CreateButtons();
                    buttonsCreated = true;
                }
            }
            else
            {
                Time.timeScale = 1;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
        }
    }

    void CreateButtons()
    {
        int buttonCount = buttonLabels.Length;
        buttons = new GameObject[buttonCount];

        // Gesamtgröße des Stacks berechnen
        float totalHeight = buttonCount * buttonSize.y + (buttonCount - 1) * buttonSpacing;
        float startY = totalHeight / 2f - buttonSize.y / 2f;

        for (int i = 0; i < buttonCount; i++)
        {
            GameObject button = new GameObject($"Button_{i + 1}", typeof(RectTransform), typeof(Image), typeof(Button));
            button.transform.SetParent(escTransform, false);

            RectTransform rt = button.GetComponent<RectTransform>();
            rt.sizeDelta = buttonSize;

            // Zentrieren
            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
            float posY = startY - i * (buttonSize.y + buttonSpacing);
            rt.anchoredPosition = new Vector2(0, posY);

            // Sichtbares Button-Design
            Image img = button.GetComponent<Image>();
            img.color = new Color(0.9f, 0.9f, 0.9f, 1f);

            // Text hinzufügen
            GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textObj.transform.SetParent(button.transform, false);

            RectTransform textRT = textObj.GetComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = Vector2.zero;
            textRT.offsetMax = Vector2.zero;

            Text txt = textObj.GetComponent<Text>();
            txt.text = buttonLabels[i];
            txt.alignment = TextAnchor.MiddleCenter;
            txt.fontSize = 30;
            txt.color = Color.black;
            txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            

            // Klickfunktion hinzufügen
            int index = i;
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(index));

            buttons[i] = button;
        }

        Debug.Log($"✅ {buttons.Length} Buttons erstellt.");
    }

    void OnButtonClicked(int index)
    {
        string label = buttonLabels[index];
        Debug.Log($"Button '{label}' wurde gedrückt!");

        switch (label)
        {
            case "Weiter":
                // Menü schließen
                isEscOpen = !isEscOpen;
                escTransform.gameObject.SetActive(isEscOpen);

                if (isEscOpen)
                {
                    Time.timeScale = 0;

                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.visible = true;

                    if (!buttonsCreated)
                    {
                        CreateButtons();
                        buttonsCreated = true;
                    }
                }
                else
                {
                    Time.timeScale = 1;
                    UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                    UnityEngine.Cursor.visible = false;
                }
                break;

            case "Einstellungen":
                // Beispiel – öffne ein Options-Menü
                Debug.Log("Einstellungsmenü öffnen...");
                break;

            case "Hauptmenü":
                // Beispiel – Szene wechseln
                Debug.Log("Zurück zum Hauptmenü...");
                // SceneManager.LoadScene("MainMenu"); // <-- wenn du das möchtest
                break;
        }
    }
}
