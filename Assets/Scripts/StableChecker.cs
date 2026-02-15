using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class StableChecker : MonoBehaviour //자체적으로 계속 시뮬을 돌려서 확인함, cliptosurface랑 상관없음!
{
    [Tooltip("physics.simulate 를 이 숫자만큼 반복해 돌려서 확인함")]
    [SerializeField] private int repeat = 30;
    [SerializeField] private float step = 0.1f;

    private Vector3 savePos;
    Quaternion saveRot;
    Deploy parentDeploy;

    private Rigidbody rb;
    private Collider col;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.parent != null)
        {
            parentDeploy = transform.parent.GetComponent<Deploy>();
            rb = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            transform.localScale = Vector3.one;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        bool canDeploy = parentDeploy && parentDeploy.isDeploy && parentDeploy.isStableChange;
        if(canDeploy)
            StableCheck();
    }

    void StableCheck()
    {
        Physics.simulationMode = SimulationMode.Script;
        rb.isKinematic = false;
        col.isTrigger = false;
        transform.position = transform.parent.position;
        transform.rotation = transform.parent.rotation;
        savePos = transform.position;
        saveRot = transform.rotation;
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        for (int i = 0; i < repeat; i++)
            Physics.Simulate(step);
        bool isPosRotSame = transform.position == savePos && transform.rotation == saveRot;
        isPosRotSame = Vector3.Distance(transform.position, savePos) < 0.001f && Quaternion.Angle(transform.rotation, saveRot) < 0.01f; //해보고 값 바꾸기!
        bool isVelocityStable = rb.linearVelocity.sqrMagnitude < 0.01f && rb.angularVelocity.magnitude < 0.01f;
        parentDeploy.isStable = isPosRotSame; //TODO 이거 어떻게 손볼지 고민해봐야 함
        Physics.simulationMode = SimulationMode.FixedUpdate;
        parentDeploy.isStableChange = false;
    }

    void PosRotReset() //이거는 왜 만든거지
    { 
        transform.position = savePos;
        transform.rotation = saveRot;
    }
    
    public void ChildDeployEnd()
    {
        transform.position = savePos;
        transform.rotation = saveRot;
        rb.isKinematic = true;
        gameObject.SetActive(false);
    }
    
}
