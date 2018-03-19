using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class XMLController : MonoBehaviour {

    public static XMLController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            resultContainer = new ResultContainer();
            resultContainer.Results = new List<Result>();
        }
    }

    public ResultContainer resultContainer;
  
}
