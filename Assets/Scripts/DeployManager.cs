using UnityEngine;

public class DeployManager : MonoBehaviour
{
    public DeployObjects[] deployObjects;

    [SerializeField] UIManager uiManager;
    [SerializeField] Vector3 initialDeployPos;

    GameObject DeployingObjectPrefab = null;
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

    void Deploy() //TODO 맞다 그리고 큐브 오브젝트는 괜찮은데 도미노 오브젝트는 다른 오브젝트 위에 올리는게 작동을 안하네 그리고 배치하려다가 인벤토리버튼 누르면 걍 거기에서 배치되버리는 문제도 있는듯
    
    {
        if (uiManager.activeInventoryButtonIndex == DeployingObjectIndex) //선택되어있는 버튼이 바로 전 버튼과 같으면 걍 끝
        {
            return;
        }

        if (uiManager.activeInventoryButtonIndex == null) //선택되어 있는 버튼이 없다면
        {
            Destroy(DeployingObject); // 아 여기서 생성된 오브젝트가 아니라 프리팹을 삭제하고 있었네!!! 근데 제대로 하니까 또 배치가 안됌
            print("여기서 오브젝트 삭제 됬어야 함!");
            DeployingObjectPrefab = null;
            DeployingObjectIndex = null;
        }
        else if(deployObjects[uiManager.activeInventoryButtonIndex.Value].count > 0) //새로운 버튼이고 배치할 오브젝트 수가 남아있다면 실행
        {
            Destroy(DeployingObject);
            if(deployObjects[uiManager.activeInventoryButtonIndex.Value].count < 1) //배치할 오브젝트 수가 남아있지 않다면 끝
                return;
            DeployingObjectPrefab = uiManager.activeInventoryButtonIndex.HasValue
                ? deployObjects[uiManager.activeInventoryButtonIndex.Value].deployObject.gameObject
                : null;
            DeployingObjectIndex = uiManager.activeInventoryButtonIndex.Value;
            DeployingObject = Instantiate(DeployingObjectPrefab, initialDeployPos, Quaternion.identity);
        }

    }

    public void DeployFinish()
    {
        deployObjects[uiManager.activeInventoryButtonIndex.Value].count--;
        DeployingObjectIndex = null;
        DeployingObject = null;
    }

}

[System.Serializable]
public class DeployObjects
{
    public string name;
    public GameObject deployObject;
    public int count;
}