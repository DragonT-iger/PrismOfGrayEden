using UnityEngine;
using UnityEngine.UI;

public class HpStarScript : MonoBehaviour
{
    [SerializeField] private GameObject _hpStar1;
    [SerializeField] private GameObject _hpStar2;
    [SerializeField] private GameObject _hpStar3;
    public void SetHpStar(int hp)
    {
        _hpStar1.GetComponent<Toggle>().isOn = hp < 1;
        _hpStar2.GetComponent<Toggle>().isOn = hp < 2;
        _hpStar3.GetComponent<Toggle>().isOn = hp < 3;
    }
}
