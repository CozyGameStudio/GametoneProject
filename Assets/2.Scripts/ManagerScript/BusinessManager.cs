using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusinessManager : MonoBehaviour
{
    private static BusinessManager instance;
    public ScriptableMachine[] machineData;
    public ScriptableFood[] foodData;

    public static BusinessManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BusinessManager>();
            }
            return instance;
        }
    }

    public void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
