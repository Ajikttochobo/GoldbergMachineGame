using UnityEngine;

public class DeployManager : MonoBehaviour //TODO 선택해제시 오브젝트 삭제 안돼는거 고치기
{
    public DeployObjects[] deployObjects;

    [SerializeField] UIManager uiManager;
    [SerializeField] Vector3 initialDeployPos;

    GameObject PreviewDeployingObject = null;
    GameObject DeployingObject = null;
    private int? PreviewDeployingObjectIndex = null;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.isGamePlaying)
            Deploy();
        print(PreviewDeployingObject);
    }

    void Deploy()
    {
        if (uiManager.activeInventoryButtonIndex == PreviewDeployingObjectIndex)
        {
            return;
        }

        if (uiManager.activeInventoryButtonIndex == null)
        {
            Destroy(PreviewDeployingObject); // 아 여기서 생성된 오브젝트가 아니라 프리팹을 삭제하고 있었네!!! 근데 제대로 하니까 또 배치가 안됌
            print("여기서 오브젝트 삭제 됬어야 함!");
            PreviewDeployingObject = null;
            PreviewDeployingObjectIndex = null;
        }
        else if(deployObjects[uiManager.activeInventoryButtonIndex.Value].count > 0)
        {
            Destroy(PreviewDeployingObject);
            if(deployObjects[uiManager.activeInventoryButtonIndex.Value].count < 1)
                return;
            PreviewDeployingObject = uiManager.activeInventoryButtonIndex.HasValue
                ? deployObjects[uiManager.activeInventoryButtonIndex.Value].deployObject.gameObject
                : null;
            PreviewDeployingObjectIndex = uiManager.activeInventoryButtonIndex.Value;
            Instantiate(PreviewDeployingObject, initialDeployPos, Quaternion.identity);
        }

    }

    public void DeployFinish()
    {
        deployObjects[uiManager.activeInventoryButtonIndex.Value].count--;
        PreviewDeployingObjectIndex = null;
    }

}

[System.Serializable]
public class DeployObjects
{
    public string name;
    public GameObject deployObject;
    public int count;
}