using System.Collections;
using System.Threading.Tasks;
using Main;
using Mechanics;
using Network;
using UI;
using UnityEngine;
using UnityEngine.Networking;


namespace Characters
{
    public class ShipController : NetworkMovableObject
    {
        public string PlayerName
        {
            get => playerName;
            set => playerName = value;
        }

        protected override float speed => shipSpeed;

        [SerializeField] private Transform cameraAttach;
        private CameraOrbit cameraOrbit;
        private PlayerLabel playerLabel;
        private float shipSpeed;
        private Rigidbody rb;

        [SyncVar] private string playerName;

        private void OnGUI()
        {
            if (cameraOrbit == null)
            {
                return;
            }
            cameraOrbit.ShowPlayerLabels(playerLabel);
        }

        public override void OnStartAuthority()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                return;
            }
            gameObject.name = playerName;
            cameraOrbit = FindObjectOfType<CameraOrbit>();
            cameraOrbit.Initiate(cameraAttach == null ? transform : cameraAttach);
            playerLabel = GetComponentInChildren<PlayerLabel>();
            base.OnStartAuthority();
        }

        protected override void HasAuthorityMovement()
        {
            var spaceShipSettings = SettingsContainer.Instance?.SpaceShipSettings;
            if (spaceShipSettings == null)
            {
                return;
            }

            var isFaster = Input.GetKey(KeyCode.LeftShift);
            var speed = spaceShipSettings.ShipSpeed;
            var faster = isFaster ? spaceShipSettings.Faster : 1.0f;

            shipSpeed = Mathf.Lerp(shipSpeed, speed * faster,
                SettingsContainer.Instance.SpaceShipSettings.Acceleration);

            var currentFov = isFaster
                ? SettingsContainer.Instance.SpaceShipSettings.FasterFov
                : SettingsContainer.Instance.SpaceShipSettings.NormalFov;
            cameraOrbit.SetFov(currentFov, SettingsContainer.Instance.SpaceShipSettings.ChangeFovSpeed);

            var velocity = cameraOrbit.transform.TransformDirection(Vector3.forward) * shipSpeed;
            rb.velocity = velocity * Time.deltaTime;

            if (!Input.GetKey(KeyCode.C))
            {
                var targetRotation = Quaternion.LookRotation(
                    Quaternion.AngleAxis(cameraOrbit.LookAngle, -transform.right) *
                    velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            }

            if (Input.GetKeyUp(KeyCode.V))
            {
                StartCoroutine(RotateObject(5f, 360.0f));
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                RotateObjectAsync(2f, -360.0f);
            }
        }

        protected override void FromServerUpdate() { }
        protected override void SendToServer() { }

        [ClientCallback]
        private void LateUpdate()
        {
            cameraOrbit?.CameraMovement();
            gameObject.name = playerName; 
        }

        [ClientCallback]
        public void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
            transform.position = new Vector3(100, 100, 100);
            gameObject.SetActive(true);
        }

        [ClientCallback]
        public IEnumerator RotateObject(float duration, float angle)
        {
            float startRotation = transform.eulerAngles.z;
            float endRotation = startRotation + angle;
            float t = 0.0f;

            while (t < duration)
            {
                t += Time.deltaTime;

                float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % angle;

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                zRotation);

                yield return null;
            }
        }

        [ClientCallback]
        public async void RotateObjectAsync(float duration, float angle)
        {
            float startRotation = transform.eulerAngles.z;
            float endRotation = startRotation + angle;
            float t = 0.0f;

            while (t < duration)
            {
                t += Time.deltaTime;

                float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % angle;

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                zRotation);

                await Task.Yield();
            }
        }
    }
}
