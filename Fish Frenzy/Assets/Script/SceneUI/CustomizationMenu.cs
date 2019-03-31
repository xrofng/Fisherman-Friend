﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CustomizationMenu : MonoBehaviour
{
    enum CustomProperties
    {
        hat = 0,
        color,
        victoryintro
    };
    public int playerCustomizeMenuID;
    public float inputDelay = 0.75f;
    private bool ignorInput = false;
    public GameObject playerModel;

    [Header("Menu")]
    public RectTransform propertiesHighlight;
    public Image[] customizePropertiesImage;
    private int customizePropertesNum;
    public Image selectionPanelUIImage;
    public Image ReadyButtonUIImage;
    public Sprite[] selectionPanelImages;
    public Sprite[] ReadyButtonImages;
    public Sprite[] ReadyButtonHoverImages;
    public bool playerReady = false;

    [Header("Properties")]
    /// index of chosen custmization of all property
    public int[] customIndex = new int[3];
    /// index of highlighted property
    private int customizePropertiesIndex;
     /// length of property of all property
    private int[] propertiesLength = new int[3];
    public Sprite[] hatCustom;
    public Sprite[] colorCustom;
    public Sprite[] vicCustom;
    public Image readyBanner;

    // ref to other
    private CharacterSceneGUI _characterSceneGUI;
    private CharacterSceneGUI CharacterSceneGUI
    {
        get
        {
            if (!_characterSceneGUI)
            {
                _characterSceneGUI = FindObjectOfType<CharacterSceneGUI>();
            }
            return _characterSceneGUI;
        }
    }
    
    public void AddCustomizePropertiesIndex(int increment)
    {
        int customizeProperties_Ready = customizePropertiesImage.Length + 1;
        customizePropertiesIndex = (customizeProperties_Ready
            + customizePropertiesIndex
            + increment)
            % customizeProperties_Ready;
    }

    public void AddCustomizeIndex(int customizePropertiesIndex, int increment)
    {
        int beforeChangedIndex = customIndex[customizePropertiesIndex];
        int changedIndex = (propertiesLength[customizePropertiesIndex]
            + customIndex[customizePropertiesIndex]
            + increment)
            % propertiesLength[customizePropertiesIndex];

        if (customizePropertiesIndex == (int)CustomProperties.color)
        {
            CharacterSceneGUI.RemoveTakenId(beforeChangedIndex);
            SetSkinColorIndex(changedIndex, increment);
            return;
        }
        customIndex[customizePropertiesIndex] = changedIndex;
    }

    public void SetSkinColorIndex(int changeToIndex, int increment)
    {
        customIndex[(int)CustomProperties.color] = changeToIndex;
        if (CharacterSceneGUI.takenSkinColorId.Contains(changeToIndex))
        {
            CharacterSceneGUI.takenSkinColorId.Add(changeToIndex);
            AddCustomizeIndex((int)CustomProperties.color, increment);
        }else
        {
            customIndex[(int)CustomProperties.color] = changeToIndex;
            CharacterSceneGUI.takenSkinColorId.Add(changeToIndex);
        }
        UpdateCustomizeImage();
    }

    void Awake()
    {
        if(propertiesLength.Length != customizePropertiesImage.Length ||
            customizePropertiesImage.Length != customIndex.Length ||
            propertiesLength.Length != customIndex.Length)
        {
            Debug.LogWarning("length no equal");
        }
        propertiesLength[(int)CustomProperties.color] = colorCustom.Length;
        propertiesLength[(int)CustomProperties.hat] = hatCustom.Length;
        propertiesLength[(int)CustomProperties.victoryintro] = vicCustom.Length;
        customizePropertesNum = customizePropertiesImage.Length;

        selectionPanelUIImage.sprite = selectionPanelImages[playerCustomizeMenuID];
        ReadyButtonUIImage.sprite = ReadyButtonImages[playerCustomizeMenuID];

        UpdatePropertiesHighlight();

    }

    void Update()
    {
        ChangeCustomizeProperties();
        PressReady();
    }

    private void PressReady()
    {
        if (!CheckPropertiesIndexIsAtReady())
        {
            return;
        }
        if (JoystickManager.Instance.GetButtonDown("Jump", playerCustomizeMenuID) && !playerReady)
        {
            playerReady = true;
        }
        if (JoystickManager.Instance.GetButtonDown("Fishing", playerCustomizeMenuID))
        {
            playerReady = false;
        }
        readyBanner.gameObject.SetActive(playerReady);
        CharacterSceneGUI.CheckAllPlayerReady();
    }

    /// <summary>
    /// 
    /// </summary>
    void ChangeCustomizeProperties()
    {
        if (ignorInput || playerReady)
        {
            return;
        }

        float axisRawX = JoystickManager.Instance.GetAxisRaw("Dhori", playerCustomizeMenuID);
        float axisRawY = JoystickManager.Instance.GetAxisRaw("Dverti", playerCustomizeMenuID);

        if (axisRawX == 0.0f) { axisRawX = JoystickManager.Instance.GetAxisRaw("Hori", playerCustomizeMenuID); }
        if (axisRawY == 0.0f) { axisRawY = JoystickManager.Instance.GetAxisRaw("Verti", playerCustomizeMenuID); }

        if (sClass.intervalCheck(axisRawX, -0.9f, 0.9f, true) && !CheckPropertiesIndexIsAtReady())
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
        if(customizePropertiesIndex >= customizePropertiesImage.Length)
        {
            propertiesHighlight.position = Vector3.one * 10000;
            ReadyButtonUIImage.sprite = ReadyButtonHoverImages[playerCustomizeMenuID];
        }
        else
        {
            ReadyButtonUIImage.sprite = ReadyButtonImages[playerCustomizeMenuID];
            propertiesHighlight.position = customizePropertiesImage[customizePropertiesIndex].rectTransform.position;
        }
    }

    void UpdateCustomizeImage()
    {
        customizePropertiesImage[(int)CustomProperties.hat].sprite = hatCustom[customIndex[(int)CustomProperties.hat]];
        customizePropertiesImage[(int)CustomProperties.color].sprite = colorCustom[customIndex[(int)CustomProperties.color]];
        customizePropertiesImage[(int)CustomProperties.victoryintro].sprite = vicCustom[customIndex[(int)CustomProperties.victoryintro]];
        MaterialManager.Instance.GetChangedColorPlayer(playerModel, customIndex[(int)CustomProperties.color]);
        PlayerData.Instance.playerSkinId[playerCustomizeMenuID] = customIndex[(int)CustomProperties.color];
    }

    IEnumerator ieIgnoreInput()
    {
        ignorInput = true;
        yield return new WaitForSeconds(inputDelay);
        ignorInput = false;
    }

    bool CheckPropertiesIndexIsAtReady()
    {
        return customizePropertiesIndex == customizePropertesNum;
    }
}


