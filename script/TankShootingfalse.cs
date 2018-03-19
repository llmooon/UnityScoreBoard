using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShootingfalse : MonoBehaviour
    {

        GameObject tmp;
        private socketRet tmp2;
        public int seq; //seq를 기준으로 터렛을 구분함.
        public string tf;
        public bool flag = false;       
        public Rigidbody m_Shell;                
        public Transform m_FireTransform;          
        public float m_MaxLaunchForce = 30f;      
        public float m_MaxChargeTime = 0.75f;       

        private float m_CurrentLaunchForce;       
        private float m_ChargeSpeed;              
        private bool m_Fired;                      

        private void OnEnable()
        {
            m_CurrentLaunchForce = m_MinLaunchForce;
        }

        private void Start()
        {
            tmp = GameObject.Find("TEMP");
            tmp2 = tmp.GetComponent<socketRet>();
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }

        private void Update()
        {
            if(tmp2.SocketRetTF == "F")
            {
                flag = true;
            }
            if (flag && tmp2.SocketRetTFseq == seq)
            {
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;
                m_CurrentLaunchForce += m_ChargeSpeed * 1.2f;
                Fire();
            }
        }
        private void Fire()
        {
            m_Fired = true;
            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward * 0.35f;
            m_CurrentLaunchForce = m_MinLaunchForce;
            flag = false;
            tmp2.SocketRetTF = "V";
            tmp2.SocketRetTFseq = 13;
        }
    }
}