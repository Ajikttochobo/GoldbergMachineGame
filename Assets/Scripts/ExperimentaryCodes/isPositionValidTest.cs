using Unity.Content;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Collider objCol;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objCol = GetComponent<Collider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        print(isPositionValid(transform.position));
    }

    private bool isPositionValid(Vector3 pos) //ㅇㅋ 일단 작동은 하네
    {
        Collider[] colliders = Physics.OverlapBox(pos, objCol.bounds.extents, Quaternion.identity);
        Vector3 dir;
        float dis;
        foreach (Collider col in colliders)
        {
            if(col == objCol)
                continue;
            Physics.ComputePenetration(objCol, pos, Quaternion.identity, col, col.transform.position, col.transform.rotation, out dir, out dis);
            if(dis > 0.001f)
                return true;
        }
        return false;
    }
}
