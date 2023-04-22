using UnityEngine;
using UnityEngine.UI;

public class nn : MonoBehaviour
{
    [SerializeField] Toggle _nn;
    private void Start() => _nn.SetIsOnWithoutNotify(PlayerPrefs.GetInt("nn", 0) == 1);
    public void NN(bool input) => PlayerPrefs.SetInt("nn", input == true ? 1 : 0);
}
