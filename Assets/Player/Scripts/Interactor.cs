using UnityEngine;

public interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;

    private GameObject previewObject;

    void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                GameObject original = hitInfo.collider.gameObject;
                Vector3 scale = original.transform.localScale;

                // Berechne Mittelpunkt der Oberseite
                Vector3 topCenter = original.transform.position + original.transform.up * (scale.y / 2f);

                // Richtung vom Mittelpunkt der Oberseite zum Trefferpunkt (nur X und Z relevant)
                Vector3 direction = hitInfo.point - topCenter;
                direction.y = 0; // Nur horizontale Richtung

                // Bestimme die Seite mit dem größten Anteil
                Vector3 sideOffset;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    // Rechts oder Links
                    sideOffset = (direction.x > 0 ? original.transform.right : -original.transform.right) * scale.x;
                }
                else
                {
                    // Vorne oder Hinten
                    sideOffset = (direction.z > 0 ? original.transform.forward : -original.transform.forward) * scale.z;
                }

                Vector3 spawnPosition = original.transform.position + sideOffset;

                // Vorschau-Objekt erzeugen oder bewegen
                if (previewObject == null)
                {
                    previewObject = Instantiate(original, spawnPosition, original.transform.rotation);
                    previewObject.name = "PreviewObject";
                    SetPreviewMaterial(previewObject);
                }
                else
                {
                    previewObject.transform.position = spawnPosition;
                    previewObject.transform.rotation = original.transform.rotation;
                }

                // Bei Tastendruck echtes Objekt erzeugen
                if (Input.GetKeyDown(KeyCode.T))
                {
                    GameObject clone = Instantiate(original, spawnPosition, original.transform.rotation);
                    clone.name = original.name;
                }
            }
            else
            {
                DestroyPreview();
            }
        }
        else
        {
            DestroyPreview();
        }
    }

    private void SetPreviewMaterial(GameObject obj)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                // Setze Rendering-Mode auf Transparent
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                // Setze Farbe auf halbtransparentes Grün
                mat.color = new Color(0f, 1f, 0f, 0.5f);
            }
        }
    }

    private void DestroyPreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }
}
