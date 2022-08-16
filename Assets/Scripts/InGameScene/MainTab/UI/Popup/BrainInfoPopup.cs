﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MainTab;

namespace InGame
{
    /// <summary>
    /// 브레인의 정보를 나타내는 팝업 클래스<br />
    /// 브레인 판매 가능<br />
    /// </summary>
    public class BrainInfoPopup : PopupBase
    {
        [SerializeField] private Button _sellBtn;
        [SerializeField] private Brain _brain;
        [SerializeField] private TextMeshProUGUI _idText;
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _intellectText;
        [SerializeField] private TextMeshProUGUI _npText;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _upgradeCost;
        [SerializeField] private TextMeshProUGUI _decomposeReward;
        public void Init(Brain brain)
        {
            base.Init();
            _brain = brain;

        }

        private Hashtable _sendData = new Hashtable();

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            UpArrowNotation storedNP = Exchange.GetNPRewardForBrainDecomposition(_brain.Intellect);
            _idText.text = _brain.ID.ToString();
            switch (_brain.Type)
            {
                case EBrainType.MAINBRAIN:
                    _typeText.text = "Core Brain";
                    break;
                case EBrainType.NORMALBRAIN:
                    _typeText.text = "Normal Brain";
                    break;
                default:
                    _typeText.text = "Unknown";
                    break;
            }
            _intellectText.text = _brain.Intellect.ToString();
            _npText.text = storedNP.ToString();
            _distanceText.text = _brain.Distance.ToString();

            _upgradeCost.text = string.Format("Upgrade\nCost: {0} NP", 1);              // 업그레이드 비용 계산해서 표시
            _decomposeReward.text = string.Format("Decompose\nfor {0} NP", storedNP);   // "총" 획득 NP량 계산해서 표시
        }
        public override void Dispose()
        {
            base.Dispose();
            NotificationManager.Instance.PostNotification(ENotiMessage.CLOSE_BRAININFO_POPUP);
        }

        public void OnClick_SellBrain()
        {
            _sendData.Clear();
            _sendData.Add(EDataParamKey.CLASS_BRAIN, _brain);
            NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_SELL_BRAIN, _sendData);
            Dispose();
        }

        public void OnClick_UpgradeBrain()
        {
            _sendData.Clear();
            _sendData.Add(EDataParamKey.BRAIN_ID, _brain.ID);
            NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_UPGRADE_BRAIN, _sendData);
        }

    }
}
