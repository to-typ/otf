using UnityEngine;

public interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform lenkrad;

    public Transform InteractorSource;
    public float InteractRange;

    private GameObject previewObject;
    private bool canPlace = true; // Neu: Status, ob gebaut werden darf

    void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                GameObject original = hitInfo.collider.gameObject;
                Vector3 scale = original.transform.localScale;

                Vector3 topCenter = original.transform.position + original.transform.up * (scale.y / 2f);
                Vector3 direction = hitInfo.point - topCenter;
                direction.y = 0;

                Vector3 sideOffset;
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    sideOffset = (direction.x > 0 ? original.transform.right : -original.transform.right) * scale.x;
                }
                else
                {
                    sideOffset = (direction.z > 0 ? original.transform.forward : -original.transform.forward) * scale.z;
                }

                Vector3 spawnPosition = original.transform.position + sideOffset;

                if (previewObject == null)
                {
                    previewObject = Instantiate(original, spawnPosition, original.transform.rotation);
                    previewObject.name = "PreviewObject";
                }
                else
                {
                    previewObject.transform.position = spawnPosition;
                    previewObject.transform.rotation = original.transform.rotation;
                }

                // Prüfe, ob Collider mit anderen Objekten überlappt
                Collider previewCollider = previewObject.GetComponent<Collider>();
                if (previewCollider is BoxCollider boxPreview)
                {
                    Collider[] overlaps = Physics.OverlapBox(
                        boxPreview.bounds.center,
                        boxPreview.bounds.extents,
                        previewObject.transform.rotation,
                        ~0,
                        QueryTriggerInteraction.Ignore
                    );

                    float previewVolume = boxPreview.bounds.size.x * boxPreview.bounds.size.y * boxPreview.bounds.size.z;
                    float totalOverlapVolume = 0f;

                    foreach (var col in overlaps)
                    {
                        if (col.gameObject == previewObject) continue;
                        if (col is BoxCollider boxOther)
                        {
                            Bounds a = boxPreview.bounds;
                            Bounds b = boxOther.bounds;

                            // Schnittmenge berechnen
                            float xOverlap = Mathf.Max(0, Mathf.Min(a.max.x, b.max.x) - Mathf.Max(a.min.x, b.min.x));
                            float yOverlap = Mathf.Max(0, Mathf.Min(a.max.y, b.max.y) - Mathf.Max(a.min.y, b.min.y));
                            float zOverlap = Mathf.Max(0, Mathf.Min(a.max.z, b.max.z) - Mathf.Max(a.min.z, b.min.z));
                            float overlapVolume = xOverlap * yOverlap * zOverlap;

                            totalOverlapVolume += overlapVolume;
                        }
                    }

                    float overlapPercent = totalOverlapVolume / previewVolume;

                    if (overlapPercent > 0.5f)
                    {
                        SetPreviewMaterial(previewObject, Color.red, 0.5f);
                        canPlace = false;
                    }
                    else
                    {
                        SetPreviewMaterial(previewObject, Color.green, 0.5f);
                        canPlace = true;
                    }
                }
                else
                {
                    // Fallback für andere Collider-Typen
                    SetPreviewMaterial(previewObject, Color.green, 0.5f);
                    canPlace = true;
                }

                // Platzieren nur erlauben, wenn kein Objekt im Collider ist
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    GameObject clone = Instantiate(original, spawnPosition, original.transform.rotation);
                    clone.name = original.name;

                    // Füge das neue Objekt als Child zu "Raft" hinzu
                    GameObject raft = GameObject.Find("Raft");
                    if (raft != null)
                    {
                        clone.transform.SetParent(raft.transform);
                    }
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

    // Überladen: Farbe und Transparenz als Parameter
    private void SetPreviewMaterial(GameObject obj, Color color, float alpha)
    {
        foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                mat.color = new Color(color.r, color.g, color.b, alpha);
            }
        }
    }

    // Alte Version für Kompatibilität (optional, kann entfernt werden)
    private void SetPreviewMaterial(GameObject obj)
    {
        SetPreviewMaterial(obj, Color.green, 0.5f);
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
