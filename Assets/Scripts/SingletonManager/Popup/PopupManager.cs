﻿using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

public class PopupManager
{
    #region lifeCycle
    public void Init()
    {
        SetCanvas();

        foreach (PopupType type in Enum.GetValues(typeof(PopupType)))
        {
            if (!_stackDic.ContainsKey(type))
                _stackDic.Add(type, new Stack<PopupBase>());
        }

        foreach (var item in _stackDic)
        {
            foreach (var popup in item.Value)
            {
                popup.Init();
            }
        }
    }

    public void AdvanceTime(float dt_sec)
    {
        foreach (var item in _stackDic)
        {
            foreach (var popup in item.Value)
            {
                if (popup != null)
                {
                    popup.AdvanceTime(dt_sec);
                }
            }
        }
    }
    #endregion

    #region private field
    private Canvas _canvas = null;
    private GameObject go = null;
    private Dictionary<PopupType, Stack<PopupBase>> _stackDic = new Dictionary<PopupType, Stack<PopupBase>>();
    [ShowInInspector] private Dictionary<PopupType, GameObject> _groupDic = new Dictionary<PopupType, GameObject>();
    #endregion

    #region private methods
    private void SetCanvas()
    {
        CreateCanvas();
    }

    private void CreateCanvas()
    {
        if (_canvas != null)
            return;
        GameObject go = GameObject.Find("PopupCanvas");
        if (go == null)
            go = Managers.Pool.GrabPrefabs(EPrefabsType.POPUP, "PopupCanvas", Managers.ManagerObj.transform);

        PopupCanvas popupCanvas = go.GetComponent<PopupCanvas>();
        if(popupCanvas != null)
        {
            _canvas = popupCanvas.Canvas;
            if (!_groupDic.ContainsKey(PopupType.NORMAL))
                _groupDic.Add(PopupType.NORMAL, popupCanvas.NomalGroup);
            if (!_groupDic.ContainsKey(PopupType.IMPORTANT))
                _groupDic.Add(PopupType.IMPORTANT, popupCanvas.ImportantGroup);
        }
        else
        {
            Debug.LogError("not canvas");
        }
    }

    private void OnNotification(Notification noti)
    {
    }

    private GameObject CreatePopupObj(EPrefabsType type, string name, Transform layer)
    {
        go = Managers.Pool.GrabPrefabs(type, name, layer);
        go.transform.position = layer.position;
        go.transform.localScale = new Vector3(1, 1, 1);
        return go;
    }
    #endregion

    #region property
    public int Count
    {
        get
        {
            int ans = 0;
            foreach (var item in _stackDic)
            {
                ans += item.Value.Count;
            }
            return ans;
        }
    }

    public int NormalCount
    {
        get
        {
            return _stackDic[PopupType.NORMAL].Count;
        }
    }

    public int ImportantCount
    {
        get
        {
            return _stackDic[PopupType.IMPORTANT].Count;
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// 팝업 생성 메서드<br />
    /// </summary>
    /// <param name="type">pool manager prefab type</param>
    /// <param name="name">popup prefab name</param>
    /// <param name="popupType">normal, improtant type</param>
    /// <returns></returns>
    public GameObject CreatePopup(EPrefabsType type, string name, PopupType popupType)
    {
        if (_canvas == null)
        {
            Debug.LogError("[Self] expected canvas");
            CreateCanvas();
        }

        go = CreatePopupObj(type, name, _groupDic[popupType].transform);
        PopupBase popup = go.GetComponent<PopupBase>();
        _stackDic[popupType].Push(popup);
        popup.Init();
        popup.PopupType = popupType;
        return go;
    }

    /// <summary>
    /// Pool manager에서 despawn을 호출하지 않고 stack에서만 지움<br />
    /// popupBase만 사용. 안쓰면 됩니다<br />
    /// </summary>
    /// <param name="type"></param>
    public void DeleteCall(PopupType type)
    {
        if (_stackDic[type].Count > 0)
            _stackDic[type].Pop();
    }

    /// <summary>
    /// popup type에 해당하는 팝업 오브젝트 하나 삭제 <br />
    /// popupBase의 dispose안에서 호출 X.의도하지 않은 다른 팝업을 삭제 할 수 있음. <br />
    /// 외부에서 사용하는 메서드 ex) 다른 manager라던지... <br />
    /// </summary>
    /// <param name="type"></param>
    public void Delete(PopupType type)
    {
        if (_stackDic[type].Count > 0)
            _stackDic[type].Pop().Dispose();
    }

    /// <summary>
    /// type에 해당하는 stack, group에서 전체 삭제
    /// </summary>
    /// <param name="type"></param>
    public void DeleteAll(PopupType type)
    {
        while (_stackDic[type].Count > 0)
            _stackDic[type].Peek().Dispose(); 
    }

    /// <summary>
    /// 모든 popup 삭제
    /// </summary>
    public void DeleteAll()
    {
        foreach (var item in _stackDic)
        {
            DeleteAll(item.Key);
        }
        
    }
    #endregion
}
public enum PopupType
{
    NORMAL = 0,
    IMPORTANT,

    UNKNOWN = 101,
}
