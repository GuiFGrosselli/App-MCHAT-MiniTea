using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AreaSegura : MonoBehaviour
{
    private RectTransform rt;
    private Rect ultimaArea;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        Aplicar();
    }

    void Update()
    {
        // reaplica se a orientação ou área mudar
        if (Screen.safeArea != ultimaArea) Aplicar();
    }

    void Aplicar()
    {
        Rect area = Screen.safeArea;
        ultimaArea = area;

        Vector2 min = area.position;
        Vector2 max = area.position + area.size;
        min.x /= Screen.width;   min.y /= Screen.height;
        max.x /= Screen.width;   max.y /= Screen.height;

        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}