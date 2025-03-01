using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static partial class UIExtensions
{
    public static bool IsPointerOverUIObject(IEnumerable<string> exceptionTags)
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        bool isOverUi = results.Any(item => !exceptionTags.Contains(item.gameObject.tag));
        return Input.GetMouseButton(0) && isOverUi;
    }
}
