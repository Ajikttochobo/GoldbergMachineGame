using System;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(StableChecker))]
[RequireComponent(typeof(OverlapChecker))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class Deploy : MonoBehaviour
{
    [Header("material settings")]
    [SerializeField] float alpha = 0.5f; // Default transparency level
    [SerializeField] string materialName = "_BaseColor";
    [Header("physics settings")]
    [SerializeField] Collider objCollider;
    [SerializeField] Rigidbody objRigidbody;
    [Header("")]

    [FormerlySerializedAs("isChild")] [HideInInspector] public bool isStableCheckerChild = false;
    [HideInInspector] public bool isOverLapCheckerChild = false;
    [HideInInspector] public bool isStable = true;
    [HideInInspector] public bool isStableChange; //stablecheker가 스테이블 바뀌었는지 확인하고 확인한 뒤에 false로 바꿔놓음
    [HideInInspector] public bool isDeploy;
    
    private Vector3 currentPos;
    private Vector3 currentObjPos;
    private bool deployedOnce = false;
    private bool deploypaused = false;
    private GameObject stableCheckerChildObject;
    private GameObject overLapCheckerChildObject;
    private Collider stableCheckerChildCollider;
    private Collider overLapCheckerChildCollider;
    private OverlapChecker overlapChecker;
    private DeployManager deployManager;


    private Renderer objRenderer;
    private Material[] materials;
    
    private bool isTouching = false;
    private int contactCount = 0;
    private int ignoreLayer;
    private RaycastHit rayHit;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            contactCount++;
            isTouching = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            contactCount--;
            isTouching = contactCount > 0;
        }
    }


    private void Start()
    {
        objCollider.isTrigger = true;
        objRigidbody.isKinematic = true;
        objRenderer = GetComponent<Renderer>();
        
        ignoreLayer = ((1 << LayerMask.NameToLayer("OverlapChecker")) | (1 << LayerMask.NameToLayer("Ignore Raycast")));
        
        materials = objRenderer.materials;

        if (isStableCheckerChild || isOverLapCheckerChild)
            ChildSetting();
        DeployStart();
        CreateChild();
        if(isOverLapCheckerChild)
            Destroy(transform.GetChild(0).gameObject);
        if (!isStableCheckerChild && !isOverLapCheckerChild)
        {
            overlapChecker = overLapCheckerChildObject.GetComponent<OverlapChecker>();
            stableCheckerChildCollider = stableCheckerChildObject.GetComponent<Collider>();
            overLapCheckerChildCollider = overLapCheckerChildObject.GetComponent<Collider>();
        }
        
        deployManager = FindFirstObjectByType<DeployManager>();
    }
    private void Update()
    {
        if (isDeploy && !deploypaused)
        {
            MousePosSanp();
            
            if(Input.GetMouseButtonDown(0) && isStable && !isStableChange)
                DeployEnd();
        }
        ShowStableState();
        Rotate();
    }

    private void ShowStableState()
    {
        foreach (Material mat in materials)
        {
            mat.SetColor(materialName, isStable ? new Color(0f, 1f, 0f, 0.3f) : new Color(1f, 0f, 0f, 0.3f));
        }
    }

    #region Deploy
    void DeployStart()
    {
        isStableChange = true;
        SetTransparent();
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        isDeploy = true;
        
        /*
        objRigidbody.isKinematic = true;
        objRigidbody.useGravity = false;
        objRigidbody.interpolation = RigidbodyInterpolation.None;
        objRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        objRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        */
    }

    void DeployEnd()
    {
        SetOpaque();
        this.gameObject.layer = LayerMask.NameToLayer("Default");
        isDeploy = false;
        deployedOnce = false;
        foreach (Material mat in materials)
        {
            mat.SetColor(materialName, Color.white); //TODO 이거 해도 색이 하얀색으로 안바뀌는거 해결하기 지피티 말로는 setopaque에서 컬러 값을 건드려서 그렇다네
            //나중에 범용성 높이기 위에 basecolor 말고도 다른거들도 지원시키기!
        }
        objRenderer.materials = materials;

        StableChecker childStableChecker = stableCheckerChildObject.GetComponent<StableChecker>();
        childStableChecker.ChildDeployEnd();
        this.gameObject.tag = "Ground";
        objCollider.isTrigger = false;
        deployManager.DeployFinish();
    }
    #endregion

    #region ChildSettings
    void CreateChild()
    {
        if (!isStableCheckerChild && !isOverLapCheckerChild)
        {
            stableCheckerChildObject = Instantiate(this.gameObject, this.transform.position, this.transform.rotation, this.transform);
            Deploy childObjDeployer = stableCheckerChildObject.GetComponent<Deploy>();
            childObjDeployer.isStableCheckerChild = true;
            overLapCheckerChildObject = Instantiate(this.gameObject, this.transform.position, this.transform.rotation, this.transform);
            childObjDeployer = overLapCheckerChildObject.GetComponent<Deploy>();
            childObjDeployer.isOverLapCheckerChild = true;
            overLapCheckerChildObject.layer = LayerMask.NameToLayer("OverlapChecker");
        }
    }

    void ChildSetting()
    {
        Renderer renderer = GetComponent<Renderer>();  
        //Destroy(renderer); //위에꺼랑 이 코드 지워서 다시 물리 시뮬용 오브젝트들 보이게 함!
        Destroy(this);
        if (isOverLapCheckerChild)
        {
            Destroy(this.GetComponent<StableChecker>());
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            Destroy(this.GetComponent<OverlapChecker>());
        }
    }
    #endregion
    void MousePosSanp() //TODO 1.collider istrigger false 로 바꿔보기 2.레이어 ignoreraycast 말고 다른거로 바꿔보기 3.목표 위치로 이동했을때 겹치면 다시 계산하게
    {
        bool found = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);
        Vector3 prevPos = transform.position;
        
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var h in hits)
        {
            if (((1 << h.collider.gameObject.layer) & ignoreLayer) != 0) 
                continue;

            rayHit = h;
            found = true;
            break;
        }
        if (!found)
        {
            return;
        }
        if(!rayHit.collider.gameObject.CompareTag("Ground"))
        {
            return;
        }
        if(Vector3.Distance(currentPos, rayHit.point) < 0.01f)
        {
            return;
        }
        if (objCollider)
        {
            prevPos = transform.position;
            currentPos = rayHit.point;
            isStableChange = true;
            float halfHeight = SetHalfHight();
                
            Vector3 targetPosition = new Vector3(
                rayHit.point.x, 
                transform.position.y, 
                rayHit.point.z);
                
            transform.position = targetPosition;
            Physics.SyncTransforms();
                
            bool isNewHigher = currentPos.y > transform.position.y - halfHeight + 0.01f;
            
            if (!isTouching || isNewHigher)
            {
                targetPosition = new Vector3(
                    rayHit.point.x, 
                    rayHit.point.y + halfHeight, 
                    rayHit.point.z);
            }
            
            Vector3 dir = (targetPosition - prevPos).normalized;
            float dis = Vector3.Distance(targetPosition, prevPos);
            transform.position = prevPos;
            Physics.SyncTransforms();
            RaycastHit sweepHit;
            Vector3 originalTargetPosition = targetPosition;
            Vector3 moveVec;
            bool isOriginalPosOk = false;

            if (isPositionValid(originalTargetPosition))
            {
                isOriginalPosOk = true;
            }
            
            for (int i = 0; i < 3; i++)
            {
                if (isOriginalPosOk)
                {
                    break;
                }
                //단순 미끄러지는거는 성공
                if(!objRigidbody.SweepTest(dir, out sweepHit, dis)) //스윕테스트 실행해서 장애물에 안 부딛히면 걍 끝
                {
                    transform.position += dir * dis;
                    break;
                }
                targetPosition = prevPos + dir * (sweepHit.distance - 0.001f); //목표 지점을 일단 부딛힌 지점으로 설정하고
                moveVec = Vector3.ProjectOnPlane((dis - sweepHit.distance + 0.001f)* dir, sweepHit.normal); //남은 이동 거리와 방향을 부딛힌 면의 벡터에 투영해서 이동할 방향과 거리 다시 계산
                transform.position = targetPosition;
                Physics.SyncTransforms(); //일단 스윕테스트에서 부딛힌 지점으로 이동
                prevPos = transform.position;

                if (moveVec.magnitude < 0.001f)
                {
                    break;
                }

                if(objRigidbody.SweepTest(moveVec, out sweepHit, moveVec.magnitude + 0.001f)) //투영해서 계산한 방향과 거리로 다시 스윕테스트 실행
                {
                    targetPosition = prevPos + moveVec.normalized * (sweepHit.distance - 0.001f); //목표 지점을 일단 부딛힌 지점으로 설정하고
                    transform.position = targetPosition;
                    Physics.SyncTransforms(); //일단 스윕테스트에서 부딛힌 지점으로 이동
                    moveVec = originalTargetPosition - transform.position; //원래 목표 지점으로 이동하는 이동할 방향과 거리 계산
                }
                else
                {
                    targetPosition += moveVec; //안 부딛히면 걍 그 위치로 이동
                    // 투영해서 이동해서 안 부딛히면 걍 그 위치로 이동시키고 마는 게 문제인거 같다!!!
                    transform.position = targetPosition;
                    Physics.SyncTransforms(); //일단 스윕테스트에서 부딛힌 지점으로 이동
                    prevPos = transform.position;
                }
                    
                //전에 여기서 현재 위치 미세하게 올려서 해결했음
                //if(i >= 1)
                    //transform.position += new Vector3(0, 0.001f, 0);
                
                moveVec = originalTargetPosition - transform.position; //원래 목표 지점으로 이동하는 이동할 방향과 거리 계산
                if (objRigidbody.SweepTest(moveVec, out sweepHit, moveVec.magnitude)) //계산한 방향과 거리로 다시 스윕테스트 실행
                {
                    targetPosition = transform.position + moveVec.normalized * (sweepHit.distance-0.001f);
                }
                else
                {
                    targetPosition += moveVec; //안 부딛히면 걍 그 위치로 이동
                }
                
                transform.position = targetPosition;
                Physics.SyncTransforms();
                prevPos = transform.position;
                
                dir = (originalTargetPosition - prevPos); //원래 목표 방향으로 가는거로 재설정
                dis = Vector3.Distance(originalTargetPosition, prevPos);
                    
            }
            transform.position = targetPosition;
        }
    }

    private float SetHalfHight()
    {
        return objCollider.bounds.extents.y;
        //TODO 다른 콜라이더 종류도 모두 지원하게!
    }
    
    private bool isPositionValid(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapBox(pos, objCollider.bounds.extents, transform.rotation);
        float dis;
        foreach (Collider col in colliders)
        {
            if(col == objCollider || col == stableCheckerChildCollider || col == overLapCheckerChildCollider) //stablecheckerchild 콜라이더인지도 확인!
                continue;
            Physics.ComputePenetration(objCollider, pos, Quaternion.identity, col, col.transform.position, col.transform.rotation, out _, out dis);
            if(dis > 0.001f)
                return false;
        }
        return true;
    }

    void Rotate()
    {
        //TODO 구현하기!
        //deploypaused = true;
    }
    
    #region Transparent&Opaque
    //Call this method to make the object transparent
    private void SetTransparent()
    {
        if (objRenderer == null) return;

        foreach (Material mat in materials)
        {
            // Set material properties for transparency
            mat.SetFloat("_Surface", 1.0f); // 1.0f is Transparent in URP
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            // Apply the alpha to the color
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            mat.color = newColor;
        }
    }
    
    // Call this method to make the object opaque
    private void SetOpaque()
    {
        if (objRenderer == null) return;

        foreach (Material mat in materials)
        {
            // Set material properties for opaque
            mat.SetFloat("_Surface", 0.0f); // 0.0f is Opaque in URP
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            // Restore the alpha to full
            Color oldColor = mat.color;
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1.0f);
            mat.color = newColor;
        }
    }
    #endregion
}