using UnityEngine;

public class DeployManager : MonoBehaviour
{
    public DeployObjects[] deployObjects;

    [SerializeField] UIManager uiManager;
    [SerializeField] Vector3 initialDeployPos;

    GameObject DeployingObject = null;
    private int? DeployingObjectIndex = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.isGamePlaying)
            Deploy();
    }

    void Deploy()
    {
        if (uiManager.activeInventoryButtonIndex == DeployingObjectIndex)
        {
            return;
        }

        if (uiManager.activeInventoryButtonIndex == null)
        {
            Destroy(DeployingObject);
            DeployingObject = null;
        }
        else
        {
            Destroy(DeployingObject);
            DeployingObject = uiManager.activeInventoryButtonIndex.HasValue
                ? deployObjects[uiManager.activeInventoryButtonIndex.Value].deployObject.gameObject
                : null;
            Instantiate(DeployingObject, initialDeployPos, Quaternion.identity);
        }

    }

    public void DeployFinish()
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