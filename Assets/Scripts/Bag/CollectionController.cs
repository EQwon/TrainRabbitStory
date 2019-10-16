using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionController : MonoBehaviour
{
    [Header("Collection")]
    public GameObject collectionPanel;

    [Header("Description")]
    public GameObject descriptionPanel;
    public Image bunnyImage;
    public Text realName;
    public Text nickName;
    public Slider affinitySlider;
    public Text affinityValueText;

    public void OpenBag()
    {
        collectionPanel.SetActive(false);
        descriptionPanel.SetActive(false);
    }

    public void SelectBunny(int num)
    {
        showBunnyInfo(num);
        descriptionPanel.SetActive(true);
    }

    private void showBunnyInfo(int num)
    {
        nickName.text = ((BunnyName)num).ToString();

        int value = GameManager.instance.Affinity[num];
        affinitySlider.value = value;
        affinityValueText.text = value.ToString();
    }
}
