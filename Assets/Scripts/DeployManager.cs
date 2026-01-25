using UnityEngine;

public class DeployManager : MonoBehaviour
{
    public DeployObjects[] deployObjects;
    
    [SerializeField] UIManager uiManager;
    [SerializeField] Vector3 initialDeployPos;
    
    GameObject DeployingObject = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class DeployObjects
{
    public string name;
    public GameObject deployObject;
    public int count;
}