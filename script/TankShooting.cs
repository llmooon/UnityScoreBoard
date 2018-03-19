using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting: MonoBehaviour
    {

        GameObject tmp;
        private socketRet tmp2;
        public int seq; //seq를 기준으로 터렛을 구분함.
        public string tf;
        public bool flag = false;

        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        //public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
        public float m_MinLaunchForce = 5f;        // The force given to the shell if the fire button is not held.
        public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
        public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.


        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.


        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            m_CurrentLaunchForce = m_MinLaunchForce;
            // m_AimSlider.value = m_MinLaunchForce;
        }


        private void Start()
        {
            tmp = GameObject.Find("TEMP");
            tmp2 = tmp.GetComponent<socketRet>();

            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }


        private void Update()
        {
            if (tmp2.SocketRetTF == "T")
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
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward * 0.35f;

            m_CurrentLaunchForce = m_MinLaunchForce;
            flag = false;
            tmp2.SocketRetTF = "V";
            tmp2.SocketRetTFseq = 9999;
        }
    }
}