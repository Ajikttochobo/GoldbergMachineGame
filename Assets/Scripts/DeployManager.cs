using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DeployManager : MonoBehaviour
{
    public DeployObjects[] deployObjects;

    [SerializeField] UIManager uiManager;
    [SerializeField] Vector3 initialDeployPos;

    GameObject DeployingObjectPrefab = null;
    GameObject DeployingObject = null;
    private int? DeployingObjectIndex = null;

    private List<DeployedObject> DeployedObjects;

    void OnEnable()
    {
        uiManager.OnPlayPauseButtonPressed.AddListener(PauseButtonFunc);
        uiManager.OnResetButtonPressed.AddListener(ResetButtonFunc);
        
        DeployedObjects = new List<DeployedObject>();
    }

    void Start()
    {
        foreach (DeployObjects deployObject in deployObjects)
        {
            if (deployObject.initialCount == 0)
                deployObject.initialCount = deployObject.count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.isGamePlaying)
            Deploy();
    }

    //TODO 1. 큐브 오브젝트는 괜찮은데 도미노 오브젝트는 다른 오브젝트 위에 올리는게 작동을 안하네
    //부모 오브젝트랑 overlapchecker(안쓰는 차일드 오브젝트) 콜라이더는 위치가 똑같은데 stablechecker오브젝트는 위치가 살짤 어긋나 있네...
    //TODO 2.  배치하려다가 인벤토리버튼 누르면 걍 거기에서 배치되버리는 문제도 있는듯 이거 전에 문제 리셋되서 다 날라가서 이러는건가? <- 이건 아닌듯
    void Deploy()
    
    {
        if (uiManager.activeInventoryButtonIndex == DeployingObjectIndex) //선택되어있는 버튼이 바로 전 버튼과 같으면 걍 끝
        {
            return;
        }

        if (uiManager.activeInventoryButtonIndex == null) //선택되어 있는 버튼이 없다면
        {
            Destroy(DeployingObject);
            DeployingObjectPrefab = null;
            DeployingObjectIndex = null;
        }
        else if(deployObjects[uiManager.activeInventoryButtonIndex.Value].count > 0) //새로운 버튼이고 배치할 오브젝트 수가 남아있다면 실행
        {
            Destroy(DeployingObject);
            if(deployObjects[uiManager.activeInventoryButtonIndex.Value].count < 1) //배치할 오브젝트 수가 남아있지 않다면 끝
                return;
            DeployingObjectPrefab = uiManager.activeInventoryButtonIndex.HasValue
                ? deployObjects[uiManager.activeInventoryButtonIndex.Value].deployObjectPrefab.gameObject
                : null;
            DeployingObjectIndex = uiManager.activeInventoryButtonIndex.Value;
            DeployingObject = Instantiate(DeployingObjectPrefab, initialDeployPos, Quaternion.identity);
        }

    }

    public void DeployFinish(GameObject obj)
    {
        deployObjects[uiManager.activeInventoryButtonIndex.Value].count--;
        DeployingObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = deployObjects[uiManager.activeInventoryButtonIndex.Value].count.ToString();
        //TODO 위에 코드 작동하게!
        DeployingObjectIndex = null;
        DeployingObject = null;

        DeployedObject deployedObject = new DeployedObject();
        deployedObject.Object = obj;
        deployedObject.Transform = obj.transform;
        DeployedObjects.Add(deployedObject);
    }

    private void PauseButtonFunc()
    {
        if(UIManager.isGamePlaying)
            return;
        print("이제 게임 모드 꺼진거 맞지?");
    }

    private void ResetButtonFunc()
    {
        foreach (DeployedObject deployedObject in DeployedObjects)
        {
            Destroy(deployedObject.Object);
        }
        DeployedObjects.Clear();
        foreach (DeployObjects deployedObject in deployObjects)
        {
            deployedObject.count = deployedObject.initialCount;
        }
    }

}

[System.Serializable]
public class DeployObjects
{
    public string name;
    [FormerlySerializedAs("deployObject")] public GameObject deployObjectPrefab;
    public int count;
    [HideInInspector] public int initialCount = 0;
}

[System.Serializable]
public class DeployedObject
{
    public GameObject Object;
    public Transform Transform;
}