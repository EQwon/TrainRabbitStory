using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum StationName { 계래, 금산, 녹림, 높점, 레볼, 말랭, 바동, 베리, 솜방, 승안, 쎈타, 우이, 월궁, 좌이, 팡요, 행운, 혜묘 };

[System.Serializable]
public class Station
{
    [SerializeField] private StationName name;
    [SerializeField] private bool isOn;
    [SerializeField] private Sprite infoImage;
    [SerializeField] private Vector2 pos;

    public StationName Name { get { return name; } }
    public Vector2 Position { get { return pos; } }
    public bool IsOn { get { return isOn; } }
    public Sprite InfoImage { get { return infoImage; } }

    public void ChangeStationState(bool on)
    {
        Debug.Log("Change " + name.ToString() + " station to " + on);
        isOn = on;
    }
}

public class MapController : MonoBehaviour
{
    [Header("Object Holder")]
    [SerializeField] private GameObject mapPanel;
    private List<GameObject> stationButtons = new List<GameObject>();
    [SerializeField] private Image infoImage;

    [Header("Resource Holder")]
    [SerializeField] private GameObject stationButtonPrefab;
    [SerializeField] private List<Station> stations;
    [SerializeField] private List<Sprite> stationImages;
    [SerializeField] private Sprite nullInfoImage;

    private void Start()
    {
        for (int i = 0; i < stations.Count; i++)
        {
            GameObject button = Instantiate(stationButtonPrefab, mapPanel.transform.GetChild(0).GetChild(0));
            RectTransform rect = button.GetComponent<RectTransform>();

            button.name = stations[i].Name.ToString() + " Station";
            button.transform.GetChild(0).GetComponent<Text>().text = stations[i].Name.ToString();
            int myNum = new int();
            myNum = i;
            button.GetComponent<Button>().onClick.AddListener(delegate { ShowInfo(myNum); });

            rect.localPosition = stations[i].Position;

            stationButtons.Add(button);
        }
    }

    public void OpenMap()
    {
        mapPanel.SetActive(true);

        for (int i = 0; i < stationButtons.Count; i++)
        {
            GameObject buttons = stationButtons[i];
            Station station = stations[i];

            if (station.IsOn) buttons.GetComponent<Image>().sprite = stationImages[0];
            else buttons.GetComponent<Image>().sprite = stationImages[1];
        }
    }

    public void ShowInfo(int i)
    {
        Station station = stations[i];
        if (station.IsOn) infoImage.sprite = stations[i].InfoImage;
        else infoImage.sprite = nullInfoImage;
    }

    public void OpenBag()
    {
        mapPanel.SetActive(false);
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Station))]
public class StationDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        float w = position.width;

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(new Rect(position.x, position.y, 15, position.height), property.FindPropertyRelative("isOn"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + 15, position.y, 0.25f*w-15, position.height), property.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + 0.25f*w, position.y, 0.35f*w, position.height), property.FindPropertyRelative("infoImage"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(position.x + 0.6f*w, position.y, 0.4f * w, position.height), property.FindPropertyRelative("pos"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif
