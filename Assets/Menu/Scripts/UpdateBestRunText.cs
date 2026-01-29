using UnityEngine;
using UnityEngine.UI;

public class UpdateBestRunText : MonoBehaviour
{
    private Text lvlStatsText;

    void Awake()
    {
        lvlStatsText = GetComponent<Text>();
    }
    public void UpdateUi()
    {
        lvlStatsText.text = PersistenceManager.Instance.GetBestRunString();
    }


}
