﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class SimulateClick
{
    private static List<RaycastResult> raycastResultList = new List<RaycastResult>();

    private static PointerEventData clickEventData;

    public static void Click(GraphicRaycaster _graphicRaycaster, Camera _camera, UnityEngine.Object _obj)
    {
        if (clickEventData == null)
        {
            clickEventData = new PointerEventData(EventSystem.current);
        }

        RectTransform target;

        if (_obj is GameObject)
        {
            target = (_obj as GameObject).transform as RectTransform;
        }
        else if (_obj is Component)
        {
            target = (_obj as Component).gameObject.transform as RectTransform;
        }
        else
        {
            throw new Exception("SimulateClick error!Unknown obj type:" + _obj);
        }

        Vector3 pos = new Vector3(target.position.x - (target.pivot.x - 0.5f) * target.lossyScale.x * target.rect.width, target.position.y - (target.pivot.y - 0.5f) * target.lossyScale.y * target.rect.height, target.position.z);

        clickEventData.position = _camera.WorldToScreenPoint(pos);

        _graphicRaycaster.Raycast(clickEventData, raycastResultList);

        if (raycastResultList.Count > 0)
        {
            GameObject go = raycastResultList[0].gameObject;

            raycastResultList.Clear();

            if (go.transform.IsChildOf(target))
            {
                ExecuteClick(go);
            }
        }
    }

    public static bool CanClick(GraphicRaycaster _graphicRaycaster, Camera _camera, UnityEngine.Object _obj)
    {
        if (clickEventData == null)
        {
            clickEventData = new PointerEventData(EventSystem.current);
        }

        RectTransform target;

        if (_obj is GameObject)
        {
            target = (_obj as GameObject).transform as RectTransform;
        }
        else if (_obj is Component)
        {
            target = (_obj as Component).gameObject.transform as RectTransform;
        }
        else
        {
            throw new Exception("SimulateClick error!Unknown obj type:" + _obj);
        }

        Vector3 pos = new Vector3(target.position.x - (target.pivot.x - 0.5f) * target.lossyScale.x * target.rect.width, target.position.y - (target.pivot.y - 0.5f) * target.lossyScale.y * target.rect.height, target.position.z);

        clickEventData.position = _camera.WorldToScreenPoint(pos);

        _graphicRaycaster.Raycast(clickEventData, raycastResultList);

        if (raycastResultList.Count > 0)
        {
            GameObject go = raycastResultList[0].gameObject;

            raycastResultList.Clear();

            if (go.transform.IsChildOf(target))
            {
                return true;
            }
        }

        return false;
    }

    public static void Click(GraphicRaycaster _graphicRaycaster, Vector3 _pos)
    {
        if (clickEventData == null)
        {
            clickEventData = new PointerEventData(EventSystem.current);
        }

        clickEventData.position = _pos;

        _graphicRaycaster.Raycast(clickEventData, raycastResultList);

        if (raycastResultList.Count > 0)
        {
            GameObject go = raycastResultList[0].gameObject;

            raycastResultList.Clear();

            ExecuteClick(go);
        }
    }

    private static void ExecuteClick(GameObject _go)
    {
        bool b = ExecuteEvents.Execute(_go, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);

        if (!b)
        {
            if (_go.transform.parent != null)
            {
                ExecuteClick(_go.transform.parent.gameObject);
            }
        }
    }
}
