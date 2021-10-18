using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class BarracksUI : MonoBehaviour
{
    public static BarracksUI Instance { get; private set; }

    [SerializeField] private Transform _queueContainer;
    [SerializeField] private Transform _queueTemplate;
    [SerializeField] private List<UnitTypeSO> _barracksUnitTypeList;

    private Barracks _barracks;

    private void Awake() {
        Instance = this;

        transform.Find("CloseBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Hide();
        };

        SetupUnitButtons();

        _queueTemplate.gameObject.SetActive(false);

        Hide();
    }

    private void Update() {
        UpdateQueue();
    }

    public void Show(Barracks barracks) {
        _barracks = barracks;
        gameObject.SetActive(true);

        UpdateQueue();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    // спизженный код
    private void SetupUnitButtons() {
        Transform unitButtonContainer = transform.Find("UnitButtonContainer");
        Transform unitButtonTemplate = unitButtonContainer.Find("Template");
        unitButtonTemplate.gameObject.SetActive(false);

        foreach (UnitTypeSO unitTypeSO in _barracksUnitTypeList) {
            Transform unitButtonTransform = Instantiate(unitButtonTemplate, unitButtonContainer);
            unitButtonTransform.gameObject.SetActive(true);

            unitButtonTransform.Find("Icon").GetComponent<Image>().sprite = unitTypeSO.Sprite;

            UnitTypeSO buildUnitTypeSO = unitTypeSO;
            unitButtonTransform.GetComponent<Button_UI>().ClickFunc = () => {
                if (_barracks == null) return; // Нет построенных бараков

                if (ResourceManager.Instance.TrySpendResource(buildUnitTypeSO.ConstructionResourceAmountCostList)) {
                    _barracks.AddUnitToQueue(buildUnitTypeSO);
                }
                else {
                    TooltipCanvas.ShowTooltip_Static("Нельзя построить юнита. Недостаточно ресурсов!", 3f);
                }
            };

            unitButtonTransform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => {
                TooltipCanvas.ShowTooltip_Static(unitTypeSO.name + "\n" + ResourceAmount.GetTooltipString(buildUnitTypeSO.ConstructionResourceAmountCostList));
            };
            unitButtonTransform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => {
                TooltipCanvas.HideTooltip_Static();
            };
        }
    }

    private void UpdateQueue() {
        if (_barracks == null) return; // Нет построенных бараков

        UtilsClass.DestroyChildren(_queueContainer, "Template");

        foreach (UnitTypeSO unitTypeData in _barracks.GetBarracksQueue()) {
            // создаем объект в юай 
            Transform queueTransform = Instantiate(_queueTemplate, _queueContainer);
            queueTransform.gameObject.SetActive(true);

            queueTransform.Find("Icon").GetComponent<Image>().sprite = unitTypeData.Sprite;
            queueTransform.Find("Mask").Find("Bar").GetComponent<Image>().fillAmount = _barracks.GetProgress();
        }
    }
}
