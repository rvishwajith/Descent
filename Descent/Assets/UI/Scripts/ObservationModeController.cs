using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Components.Species;
using Utilities;

namespace UserInterface
{
    [RequireComponent(typeof(UIDocument))]
    public class ObservationModeUI : MonoBehaviour
    {
        private VisualElement root;
        private VisualElement observationModePanel;
        private Label englishSpeciesNameLabel;
        private Label latinSpeciesNameLabel;
        private Label speciesFactLabel;

        private void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            InitUI();
            InitActions();
        }

        private void InitUI()
        {
            observationModePanel = root.Q("ObservationModePanel");
            englishSpeciesNameLabel = root.Q<Label>("EnglishName");
            latinSpeciesNameLabel = root.Q<Label>("ScientificName");
            speciesFactLabel = root.Q<Label>("FactLabel");
        }

        private void InitActions()
        {
            var cameraController = Find.CameraController;
            root.Q<Button>("ExitObservation").RegisterCallback<ClickEvent>(ExitObservation);
            root.Q<Button>("PrevFact").RegisterCallback<ClickEvent>(_ => { SwitchFact(-1); });
            root.Q<Button>("NextFact").RegisterCallback<ClickEvent>(_ => { SwitchFact(1); });

            root.Q<Button>("PrevCreature").RegisterCallback<ClickEvent>(
                _ => { cameraController.TrySwitchToRandomSpecies(); });
            root.Q<Button>("NextCreature").RegisterCallback<ClickEvent>(
                _ => { cameraController.TrySwitchToRandomSpecies(); });
        }

        public void DisplaySpeciesInfo(SpeciesInfo info)
        {
            englishSpeciesNameLabel.text = info.englishName;
            latinSpeciesNameLabel.text = info.latinName;

            if (info.facts.Length > 0)
                speciesFactLabel.text = info.facts[0].description;
        }

        private void SwitchFact(int shiftAmt = 0)
        {
            Debug.Log("UI.ObservationModeController.SwitchFact(): Shift by " + shiftAmt);
        }

        private void EnterObservation(ClickEvent e)
        {
            float transitionTime = 0.5f;
            DOVirtual.Float(0, 1, transitionTime, opacity =>
            {
                observationModePanel.style.opacity = opacity;
            }).OnComplete(() =>
            {
                observationModePanel.SetEnabled(true);
                Debug.Log("UserInterface.Controller.ExitObservation(): Finished exiting.");
            });
        }

        private void ExitObservation(ClickEvent e)
        {
            float transitionTime = 0.5f;
            DOVirtual.Float(1, 0, transitionTime, opacity =>
            {
                observationModePanel.style.opacity = opacity;
            }).OnComplete(() =>
            {
                observationModePanel.SetEnabled(false);
                Debug.Log("UserInterface.Controller.ExitObservation(): Finished exiting.");
            });
        }
    }
}

