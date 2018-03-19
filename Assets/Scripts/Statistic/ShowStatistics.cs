using UnityEngine;

public class ShowStatistics : MonoBehaviour {

    public GameObject RoundPrefab;

    private void OnEnable()
    {
        foreach (Result result in XMLController.instance.resultContainer.Results)
        {
            ResultHolder resultHolder = Instantiate(RoundPrefab, transform).GetComponent<ResultHolder>();

            resultHolder.Result.text = result.result;
            resultHolder.SpendTime.text = result.SpendTime.ToString();
        }
    }

    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
