using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FishFrenzy
{
    public class CustomizationMenu : MonoBehaviour
    {
        enum CustomProperties
        {
            hat =0,
            color,
            koeffect,
            victoryintro
        };
        public int playerCustomizeMenuID;
        public float inputDelay = 0.75f;
        private bool ignorInput = false;
        public GameObject playerModel;

        public Image propertiesHighlight;
        public Image[] customizePropertiesImage;

        [Header("Properties")]
        public int[] customIndex = new int[4];  
        public int[] propertiesLength = new int[4];
        public Sprite[] hatCustom;
        public Color[] colorCustom;
        public Sprite[] koCustom;
        public Sprite[] vicCustom;

        private int customizePropertiesIndex;

        public void AddCustomizePropertiesIndex(int increment)
        {
            customizePropertiesIndex = (customizePropertiesImage.Length
                + customizePropertiesIndex 
                + increment) 
                % customizePropertiesImage.Length;
        }

        public void AddCustomizeIndex(int customizePropertiesIndex, int increment)
        {
            customIndex[customizePropertiesIndex] = (propertiesLength[customizePropertiesIndex] 
                + customIndex[customizePropertiesIndex] 
                + increment) 
                % propertiesLength[customizePropertiesIndex];
        }

        void Start()
        {
            UpdatePropertiesHighlight();
            UpdateCustomizeImage();

        }

        void Update()
        {
            ChangeCustomizeProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        void ChangeCustomizeProperties()
        {
            if (ignorInput)
            {
                return;
            }

            float axisRawX = JoystickManager.Instance.GetAxisRaw("Hori", playerCustomizeMenuID);
            float axisRawY = JoystickManager.Instance.GetAxisRaw("Verti", playerCustomizeMenuID);
            if (sClass.intervalCheck(axisRawX, -0.9f, 0.9f, true))
            {
                AddCustomizeIndex(customizePropertiesIndex, sClass.getSign(axisRawX, 0.015f));
                UpdateCustomizeImage();
                StartCoroutine(ieIgnoreInput());
            }
            if (sClass.intervalCheck(axisRawY, -0.9f, 0.9f, true))
            {
                AddCustomizePropertiesIndex(-sClass.getSign(axisRawY, 0.015f));
                UpdatePropertiesHighlight();
                StartCoroutine(ieIgnoreInput());
            }
        }

        void UpdatePropertiesHighlight()
        {
            propertiesHighlight.rectTransform.position = customizePropertiesImage[customizePropertiesIndex].rectTransform.position;
        }

        void UpdateCustomizeImage()
        {
            customizePropertiesImage[(int)CustomProperties.color].color = colorCustom[customIndex[(int)CustomProperties.color]];
            MaterialManager.Instance.GetChangedColorPlayer(playerModel, customIndex[(int)CustomProperties.color]);
            PlayerData.Instance.playerSkinId[playerCustomizeMenuID] = customIndex[(int)CustomProperties.color];
        }

        IEnumerator ieIgnoreInput()
        {
            ignorInput = true;
            yield return new WaitForSeconds(inputDelay);
            ignorInput = false;
        }
    }
}


