using UnityEngine;
using DG.Tweening;

namespace Interactable
{
    public class Door : MonoBehaviour
    {
        [Header("Panels")]
        public Transform rightPanel = null;
        public Transform leftPanel = null;

        private float closedOffsetX = 0.07f, openOffsetX = 3.2f;
        private float openDuration = 6.8f;

        // public enum State { Closed, Closing, Opening, Open };
        // [HideInInspector] public State state;

        private int state;
        public int State
        {
            get { return state; }
            set
            {
                state = value;
                if (state == 0)
                    Close();
                else if (state == 1)
                    WillOpen();
                else if (state == 2)
                    Opened();
            }
        }

        public void Awake()
        {
            State = 0;
        }

        public void Close()
        {
            rightPanel.localPosition = new(closedOffsetX, 0, 0);
            leftPanel.localPosition = new(-closedOffsetX, 0, 0);
        }

        public void WillOpen()
        {
            GetComponent<AudioSource>().Play();
            Debug.Log("Door.Open(): Started opening.");

            leftPanel.DOMoveX(-openOffsetX, openDuration);
            rightPanel.DOMoveX(openOffsetX, openDuration).OnComplete(() =>
            {
                State = 2;
            });
        }

        public void Opened()
        {
            Debug.Log("Door.DidOpen(): Finished opened.");
        }
    }
}

