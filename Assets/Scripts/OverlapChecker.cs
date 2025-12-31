using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class OverlapChecker : MonoBehaviour //TODO deploy 자체에서 오버랩확인하는 코드 만들고 문제없으면 이 코드는 폐기
{
    private Collider _collider;
    private int overlapCount;
    private int pastOverlapCount;
    private bool wasoverlapping = false;
    private int ignorelayer;
    
    private bool isOverlap = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.parent != null)
        {
            //this.gameObject.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("OverlapChecker"); //지금 계속 오버랩체커레이어 없다고 난린데 걍 무시하고 오료 고치면 이 코드 없애자
        }
        
        _collider = GetComponent<Collider>();
        ignorelayer = 1 << LayerMask.NameToLayer("Ignore Raycast");
        pastOverlapCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        isOverlap = true;
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if((ignorelayer & (1 << other.gameObject.layer)) != 0)
            return;

        overlapCount++;
        print(other.gameObject.name);
    }
    */

    /*
    public Vector3 OverlapCheck(Vector3 pos, Quaternion rot) //
    {
        transform.position = pos;
        transform.rotation = rot;
        
        //print("피직스시뮬 시작!");
        Physics.simulationMode = SimulationMode.Script;
        
        overlapCount = 0;
        Physics.Simulate(Time.fixedDeltaTime);
        
        if (overlapCount > pastOverlapCount)
        {
            print("overlap!");
        }
        pastOverlapCount = overlapCount;
        
        Physics.simulationMode = SimulationMode.FixedUpdate;
        //print("피직스시뮬 끝!");
        return Vector3.zero;
        
    }
*/
    
    public IEnumerator OverlapCheck(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        isOverlap = false;
        
        yield return null;
        
        if(isOverlap)
            print("overlap!");
    }
    


}

