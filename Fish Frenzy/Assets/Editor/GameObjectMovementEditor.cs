using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This class adds names for each LevelMapPathElement next to it on the scene view, for easier setup
/// </summary>
[CustomEditor(typeof(GameObjectMovement), true)]
[InitializeOnLoad]
public class GameObjectMovementEditor : Editor {

    public GameObjectMovement gameObjMovementTarget
    {
        get
        {
            return (GameObjectMovement)target;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        string[] excludeProperties = new string[2];

        int propertyIndex = 0;
        if (gameObjMovementTarget.moveFactor == GameObjectMovement.MoveFactor.Speed)
        {
            excludeProperties[propertyIndex] = "moveDuration";
        }
        else if(gameObjMovementTarget.moveFactor == GameObjectMovement.MoveFactor.Time)
        {
            excludeProperties[propertyIndex] = "moveSpeed";
        }

        propertyIndex = 1;
        if (gameObjMovementTarget.move_Position == GameObjectMovement.Move_Position.From)
        {
            excludeProperties[propertyIndex] = "toPosition";
        }
        else if (gameObjMovementTarget.move_Position == GameObjectMovement.Move_Position.To)
        {
            excludeProperties[propertyIndex] = "fromPosition";
        }
        
        Editor.DrawPropertiesExcluding(serializedObject, excludeProperties);

        //DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }
}
