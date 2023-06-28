using UnityEngine;

namespace Interactable
{
    using Utilities;

    public class Door : MonoBehaviour
    {
        [Header("Panels")]
        public Transform right;
        public Transform left;

        private int state = 0; // Closed
        private float widthClosed = 0.1f, widthOpen = 3.2f, openDuration = 6.8f;

        public void Awake()
        {
            Close();
        }

        public void Close()
        {
            state = 0;
            right.localPosition = Vector3.right * widthClosed;
            left.localPosition = Vector3.left * widthClosed;
        }

        public void Open(float delay)
        {
            if (state == 0)
                Invoke("Open", delay);
        }

        public void Open()
        {
            state = 1;
            transform.GetComponent<AudioSource>().Play();
            References.Animation.Animate(
                right,
                property: "LocalPosition",
                start: right.localPosition,
                end: Vector3.right * widthOpen,
                duration: openDuration);
            References.Animation.Animate(
                left,
                property: "LocalPosition",
                start: left.localPosition,
                end: Vector3.left * widthOpen,
                duration: openDuration);
            Invoke("DidOpen", openDuration);
        }

        void DidOpen()
        {
            Debug.Log("Door opened.");
            state = 2;
        }
    }
}

