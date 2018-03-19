using UnityEngine;
using System.Collections;

public class WSP_LaserBeamWS : MonoBehaviour
{
    GameObject tmp;
    private socketRet tmp2;
    public int seq; //seq를 기준으로 터렛을 구분함.
    int cnt = 0;
    public bool flag = false;
    //  public int cnt = 0;
    public bool LaserBeamActive = false;
    public bool AutoFireForDemo = false;
    public bool UseExtendedLength = false;

    public bool UseLineOfSight = true;
    private float damageOverTimeTimer = 0;
    private float damageOverTimeFreq = 0.25f;
    private bool canDamageTarget = false;

    //private AudioSource LaserFireSoundFX;
    private bool LaserSoundPlayed = false;


    public Renderer TurretColorRender;

    public Transform LaserFireEmitPointTrans;
    public ParticleSystem LaserFireCenterEmitter;
    public float FiringParticleSize = 1;

    public Transform TargetHitTransform;
    public ParticleSystem TargetHitEmitter;
    public float HitParticleSize = 1;

    public LineRenderer InnerLaserLineRender;
    public LineRenderer OuterLaserLineRender;
    public Color LaserBeamColor;
    public Material LaserYBeamMaterial;
    private Vector3 laserStartPoint = Vector3.zero;
    private Vector3 laserEndPoint = Vector3.zero;

    public bool LaserCanFire = false;
    public bool LaserFiring = false;
    public float LaserDamage = 10;
    public float LaserDamagePerSecond = 1;

    public float InnerLaserStartWidth = 1.0f;
    public float InnerLaserEndWidth = 1.0f;
    public float OuterLaserStartWidth = 1.0f;
    public float OuterLaserEndWidth = 1.0f;

    public float LaserGrowSpeed = 50;
    public float innerLaserTileAmount = 1.0f;
    private float innerLaserStartWidth = 1.0f;
    private float innerLaserEndWidth = 1.0f;
    public float outerLaserTileAmount = 1.0f;
    private float outerLaserStartWidth = 1.0f;
    private float outerLaserEndWidth = 1.0f;

    public float FireSpeed = 20.0f;

    public bool OffsetMaterialTexture = true;
    public float ScrollSpeed = 2.5F;

    private float laserLifeTimer = 0;
    public float LaserFireTime = 0.5f;

    private float laserRechargeTimer = 0;
    public float LaserRechargeTime = 0f;
    private float fDestroyTime = 1f;
    private float fTickTime;


    public Transform CurrentTarget;
    //public void AssignNewTarget(Transform target)
    //{
    //    if (CurrentTarget == null)
    //    {
    //        CurrentTarget = target;
    //        FireLaser();
    //    }
    //}

    public void FireLaser()
    {
        if (LaserCanFire)
        {
            // Set Colors
            InnerLaserLineRender.startColor = LaserBeamColor;
            InnerLaserLineRender.endColor = LaserBeamColor;
            ParticleSystem.MainModule emitterModule = LaserFireCenterEmitter.main;
            emitterModule.startColor = LaserBeamColor;
            ParticleSystem.MainModule emitter2Module = TargetHitEmitter.main;
            emitter2Module.startColor = LaserBeamColor;
            //칼라 세팅/ 없어도 공격은 된다.
            innerLaserStartWidth = 0f;
            innerLaserEndWidth = 0f;
            outerLaserStartWidth = 0f;
            outerLaserEndWidth = 0f;
            laserLifeTimer = 0;
            if (!LaserSoundPlayed)
            {
                //LaserFireSoundFX.Play();
                LaserSoundPlayed = true;
            }
            LaserFiring = true;//false로 바꾸면 안되넹...
            laserRechargeTimer = 0;
            LaserCanFire = false;
        }
    }

    public void StopLaserFire()
    {
        ParticleSystem.EmissionModule emissionModule = LaserFireCenterEmitter.emission;
        emissionModule.rateOverTime = 0;
        LaserFiring = false;
        LaserSoundPlayed = false;
    }

    void Start()
    {
        tmp = GameObject.Find("TEMP");
        tmp2 = tmp.GetComponent<socketRet>();
        if (!AutoFireForDemo)
        {
            CurrentTarget = null;
        }

        if (LaserFireCenterEmitter != null)
        {
            ParticleSystem.EmissionModule emissionModule = LaserFireCenterEmitter.emission;
            emissionModule.rateOverTime = 0;
        }
        if (TargetHitEmitter != null)
        {
            ParticleSystem.EmissionModule emissionModule = TargetHitEmitter.emission;
            emissionModule.rateOverTime = 0;
        }
        if (InnerLaserLineRender != null)
        {
            InnerLaserLineRender.enabled = false;
        }
        if (OuterLaserLineRender != null)
        {
            OuterLaserLineRender.enabled = false;
        }
    }

    // Update is called once per frame
    private void fireonce()
    {
        FireLaser();
        //if (UseExtendedLength){LaserFireTime = 0.00000001f;}
        if (LaserFiring)
        {
         //   Debug.Log("1");
            if (laserLifeTimer < LaserFireTime)
            {

                laserLifeTimer += Time.deltaTime;
                if (!LaserSoundPlayed)
                {
                    LaserSoundPlayed = true;
                }
            }
            else
            {
                if (CurrentTarget != null) { CurrentTarget.gameObject.SendMessage("Damage", LaserDamage, SendMessageOptions.DontRequireReceiver); }
                StopLaserFire();
                flag = false;
                tmp2.SocketRetValue = 9999;
                cnt = 0;
            }
        }

        else
        {
            if (!LaserCanFire)
            {
                LaserCanFire = true;
            }
        }
        UpdateLaserBeam();
    }

    void Update()
    {

        //  t1[0] = true;
        if (tmp2.SocketRetValue == seq)
        {
            switch (tmp2.SocketRetValue)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    flag = true;
                    break;
            }
        }

        if (flag && tmp2.SocketRetValue == seq)
        {
            //    cnt++;
            //  Debug.Log("seq : " + seq + "fl " + t1[seq]);
            fireonce();
        }

    }

    public void UpdateLaserBeamTiling()
    {
        // Set Laser Beam Line Material Tile Amount
        InnerLaserLineRender.material.mainTextureScale = new Vector2(innerLaserTileAmount, 1);
        OuterLaserLineRender.material.mainTextureScale = new Vector2(outerLaserTileAmount, 1);
    }

    private void UpdateLaserBeam()
    {
        ParticleSystem.MainModule mainModule1 = LaserFireCenterEmitter.main;
        mainModule1.startSize = FiringParticleSize;
        ParticleSystem.MainModule mainModule2 = TargetHitEmitter.main;
        mainModule2.startSize = HitParticleSize;

        laserStartPoint = LaserFireEmitPointTrans.position;
        if (CurrentTarget != null && LaserFiring)
        {
            ParticleSystem.EmissionModule emissionModule = LaserFireCenterEmitter.emission;
            emissionModule.rateOverTime = 1;

            if (innerLaserStartWidth < InnerLaserStartWidth)
                innerLaserStartWidth += LaserGrowSpeed * Time.deltaTime;
            if (innerLaserEndWidth < InnerLaserEndWidth)
                innerLaserEndWidth += LaserGrowSpeed * Time.deltaTime;
            if (outerLaserStartWidth < OuterLaserStartWidth)
                outerLaserStartWidth += LaserGrowSpeed * Time.deltaTime;
            if (outerLaserEndWidth < OuterLaserEndWidth)
                outerLaserEndWidth += LaserGrowSpeed * Time.deltaTime;

            InnerLaserLineRender.startWidth = innerLaserStartWidth;
            InnerLaserLineRender.endWidth = innerLaserEndWidth;
            OuterLaserLineRender.startWidth = outerLaserStartWidth;
            OuterLaserLineRender.endWidth = outerLaserEndWidth;

            if (!InnerLaserLineRender.enabled)
                InnerLaserLineRender.enabled = true;
            if (!OuterLaserLineRender.enabled)
                OuterLaserLineRender.enabled = true;

            Vector3 targetHitPosition = CurrentTarget.position;

            if (UseLineOfSight)
            {
                targetHitPosition = CheckLOSOnTarget();
            }

            float distanceToTarget = Vector3.Distance(laserEndPoint, targetHitPosition);
            if (distanceToTarget > 1.0f)
            {
                laserEndPoint = Vector3.Lerp(laserEndPoint, targetHitPosition, FireSpeed * Time.deltaTime);
                ParticleSystem.EmissionModule emission3Module = TargetHitEmitter.emission;
                emission3Module.rateOverTime = 0;
            }
            else
            {
                laserEndPoint = targetHitPosition;
                TargetHitTransform.position = targetHitPosition;
                ParticleSystem.EmissionModule emission4module = TargetHitEmitter.emission;
                emission4module.rateOverTime = 1;
            }
            if (OffsetMaterialTexture)
            {
                float offset = Time.time * ScrollSpeed;
                InnerLaserLineRender.material.SetTextureOffset("_MainTex", new Vector2(-offset, 0));
            }
        }
        else
        {
            if (InnerLaserLineRender.enabled)
                InnerLaserLineRender.enabled = false;
            if (OuterLaserLineRender.enabled)
                OuterLaserLineRender.enabled = false;
            ParticleSystem.EmissionModule emission5module = LaserFireCenterEmitter.emission;
            emission5module.rateOverTime = 0;
            ParticleSystem.EmissionModule emission6module = TargetHitEmitter.emission;
            emission6module.rateOverTime = 0;
            laserEndPoint = laserStartPoint;
        }

        if (InnerLaserLineRender != null)
        {
            InnerLaserLineRender.SetPosition(0, laserStartPoint);
            InnerLaserLineRender.SetPosition(1, laserEndPoint);
        }
        if (OuterLaserLineRender != null)
        {
            laserStartPoint.y += 0.1f;
            laserEndPoint.y += 0.1f;
            OuterLaserLineRender.SetPosition(0, laserStartPoint);
            OuterLaserLineRender.SetPosition(1, laserEndPoint);
        }
    }

    private RaycastHit myhit = new RaycastHit();
    private Ray myray = new Ray();

    private Vector3 CheckLOSOnTarget()
    {//레이캐스트
        Vector3 hitPoint = Vector3.zero;

        Vector3 rayDirection = CurrentTarget.position - LaserFireEmitPointTrans.position;
        myray = new Ray(LaserFireEmitPointTrans.position, rayDirection);
        if (Physics.Raycast(myray, out myhit, 1000.0f))
        {
            if (canDamageTarget)
            {
                myhit.collider.gameObject.transform.root.SendMessage("Damage", LaserDamagePerSecond, SendMessageOptions.DontRequireReceiver);
                canDamageTarget = false;
            }
            hitPoint = myhit.point;
        }

        return hitPoint;
    }

}