using UnityEngine;

public class DeployManager : MonoBehaviour //TODO 인벤토리 안 오브젝트 선택 시스템 뭔가 이상함(동시에 배치될 오브젝트 여러개가 나옴)... 그리고 배치한 오브젝트 위에 다른거 안올라감... <-이거는 stablechecker 오브젝트가 밑에거 뚫고 가서 그런거네 
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
            deployObjects = null;
        }
        else if(deployObjects[uiManager.activeInventoryButtonIndex.Value].count > 0)
        {
            Destroy(DeployingObject);
            if(deployObjects[uiManager.activeInventoryButtonIndex.Value].count < 1)
                return;
            DeployingObject = uiManager.activeInventoryButtonIndex.HasValue
                ? deployObjects[uiManager.activeInventoryButtonIndex.Value].deployObject.gameObject
                : null;
            DeployingObjectIndex = uiManager.activeInventoryButtonIndex.Value;
            Instantiate(DeployingObject, initialDeployPos, Quaternion.identity);
        }

    }

    public void DeployFinish()
    {
        print("배치됨!");
        deployObjects[uiManager.activeInventoryButtonIndex.Value].count--;
        DeployingObjectIndex = null;
    }

}

[System.Serializable]
public class DeployObjects
{
    public string name;
    public GameObject deployObject;
    public int count;
}