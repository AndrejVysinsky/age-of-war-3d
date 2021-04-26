using Assets.Scripts;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitTrainCard : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IUnitUpgraded, IUnitTrainingStarted
{
    [Header("Components")]
    [SerializeField] Image unitImage;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text trainKeyText;
    [SerializeField] TMP_Text unitsInQueueText;
    [SerializeField] Image timerImage;

    [Header("Misc")]
    [SerializeField] KeyCode trainKey;
    [SerializeField] int unitIndex;

    public UnitData UnitData { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public int UnitIndex => unitIndex;

    private List<float> _unitTrainTimes;
    private List<float> _unitTrainTimesInQueue;
    private float _currentTrainTime;

    private void Start()
    {
        _unitTrainTimes = new List<float>();
        _unitTrainTimesInQueue = new List<float>();

        PlayerController = FindObjectOfType<PlayerController>();

        trainKeyText.text = trainKey.ToString();
        unitsInQueueText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(trainKey))
        {
            if (PlayerController.SpawnUnit(unitIndex))
            {
                UnitAddedToQueue(UnitData.TrainTime);
            }
        }

        ShowUnitsInQueue();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(gameObject);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlayerController.SpawnUnit(unitIndex))
        {
            UnitAddedToQueue(UnitData.TrainTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TODO: Show tooltip with unit info
        MouseCursor.Instance.SetHandCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //TODO: Hide tooltip
        MouseCursor.Instance.SetPointerCursor();
    }

    public void OnUnitUpgraded(int upgradedUnitIndex, UnitData upgradedUnitData, FactionEnum faction)
    {
        if (faction != FactionEnum.Green)
            return;

        if (upgradedUnitIndex != unitIndex)
            return;

        UnitData = upgradedUnitData;

        priceText.text = UnitData.TrainCost.ToString();

        if (UnitData.Sprite != null)
        {
            unitImage.sprite = UnitData.Sprite;
        }
    }

    private void UnitAddedToQueue(float trainTime)
    {
        if (trainTime == 0)
            return;

        _unitTrainTimesInQueue.Add(trainTime);

        if (_unitTrainTimesInQueue.Count == 0)
        {
            unitsInQueueText.text = "";
        }
        else
        {
            unitsInQueueText.text = _unitTrainTimesInQueue.Count.ToString();
        }
    }

    private void ShowUnitsInQueue()
    {
        if (_unitTrainTimes.Count > 0)
        {
            _unitTrainTimes[0] -= Time.deltaTime;

            if (_unitTrainTimes[0] <= 0)
            {
                _unitTrainTimes.RemoveAt(0);

                if (_unitTrainTimes.Count > 0)
                    _currentTrainTime = _unitTrainTimes[0];
                else
                    _currentTrainTime = 0;
            }
        }

        if (_unitTrainTimes.Count == 0)
        {
            _currentTrainTime = 0;
            timerImage.gameObject.SetActive(false);
        }
        else
        {
            if (timerImage.gameObject.activeSelf == false)
                timerImage.gameObject.SetActive(true);

            timerImage.fillAmount = _unitTrainTimes[0] / _currentTrainTime;
        }
    }

    public void OnUnitTrainingStarted(int unitIndex, FactionEnum faction)
    {
        if (faction != FactionEnum.Green)
            return;

        if (this.unitIndex != unitIndex)
            return;

        _unitTrainTimes.Add(_unitTrainTimesInQueue[0]);
        _unitTrainTimesInQueue.RemoveAt(0);

        if (_unitTrainTimesInQueue.Count == 0)
        {
            unitsInQueueText.text = "";
        }
        else
        {
            unitsInQueueText.text = _unitTrainTimesInQueue.Count.ToString();
        }

        if (_currentTrainTime == 0)
            _currentTrainTime = _unitTrainTimes[0];
    }
}