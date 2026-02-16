using System.Collections;
using UnityEngine;

public class InstructionFeedBackUI : MonoBehaviour
{
    [SerializeField] private GameObject _leftSuccess;
    [SerializeField] private GameObject _RightSuccess;

    [SerializeField] private FlagGameController _cont;

    void Start()
    {
        _cont.OnJudgeResult += OnJudge;
    }

    private void OnJudge(bool left, bool right)
    {
        if(left) StartCoroutine(ShowEffect(_leftSuccess));
        if(right) StartCoroutine(ShowEffect(_RightSuccess));
    }

    private IEnumerator ShowEffect(GameObject obj)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        obj.SetActive(false);
    }
}
