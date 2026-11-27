using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using MapStuff;
using System;

[CustomPropertyDrawer(typeof(Map))]
public class CustomMapData : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);;
        EditorGUI.PrefixLabel(position, label);
        SerializedProperty rows = property.FindPropertyRelative("matrix");

        float cellWidth = 15;
        float cellHeight = 15;
        float spacing = 1f;

        float startY = position.y + 20f;
        float currentX = position.x;

        for(int i = 0; i < rows.arraySize; i++)
        {
            SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("tiles");
            float currentY = startY;

            // Debug.Log(row.arraySize);
            for(int j = 0; j < row.arraySize; j++)
            {
                Rect cellRect = new Rect(currentX, currentY, cellWidth, cellHeight);
                
                SerializedProperty tile = row.GetArrayElementAtIndex(j);
                EditorGUI.DrawRect(cellRect, GetTileColor((TileType)tile.enumValueIndex));

                if(GUI.Button(cellRect, GUIContent.none, GUIStyle.none))
                {
                    ShowTileMenu(tile);
                }
                // tile.enumValueIndex = EditorGUI.Popup(cellRect, tile.enumValueIndex, tile.enumNames, GUIStyle.none);

                currentY += cellHeight + spacing;
                
            }
            currentX += cellWidth + spacing;
        }

        EditorGUI.EndProperty();
        Rect rowButtonRect = new Rect(position.x, startY, 25f, 20f);
        if(rows.arraySize > 0)
            rowButtonRect.y += rows.GetArrayElementAtIndex(0).FindPropertyRelative("tiles").arraySize * (cellHeight + spacing);

        if(GUI.Button(rowButtonRect, "+"))
        {
            for(int i = 0; i < rows.arraySize; i++)
            {
                SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("tiles");
                row.InsertArrayElementAtIndex(row.arraySize);
            }
        }
        rowButtonRect.x += 25f;
        if(GUI.Button(rowButtonRect, "-"))
        {
            for(int i = 0; i < rows.arraySize; i++)
            {
                SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("tiles");

                if(row.arraySize > 0)
                    row.DeleteArrayElementAtIndex(row.arraySize - 1);
            }
        }

        if(rows.arraySize > 0)
        {
            float gridHeight = rows.GetArrayElementAtIndex(0).FindPropertyRelative("tiles").arraySize * (cellHeight + spacing);

            Rect columnButtonRect = new Rect(currentX, position.y + 20f, 25f, gridHeight - spacing);
            if(GUI.Button(columnButtonRect, "+"))
            {
                rows.InsertArrayElementAtIndex(rows.arraySize);
            }
            columnButtonRect.x += 25;
            if(GUI.Button(columnButtonRect, "-"))
            {
                if(rows.arraySize > 0)
                    rows.DeleteArrayElementAtIndex(rows.arraySize - 1);
            }
        }
    }

    public Color GetTileColor(TileType type)
    {
        switch(type)
        {
            case TileType.empty:
                return new Color(64f/255f, 64f/255f, 64f/255f, 1);
            case TileType.grass:
                return Color.green;
            case TileType.sand:
                return Color.yellow;
            case TileType.playerGrass:
                return Color.white;
            case TileType.enemyGrass:
                return Color.red;
            default:
                return Color.magenta;
        }
    }
    
    private void ShowTileMenu(SerializedProperty tile)
    {
        GenericMenu menu = new GenericMenu();

        for(int i = 0; i < tile.enumNames.Length; i++)
        {
            int index = i;
            menu.AddItem(new GUIContent(tile.enumNames[i]), tile.enumValueIndex == index, () =>
            {
                tile.enumValueIndex = index;
                tile.serializedObject.ApplyModifiedProperties();
            });
        }

        menu.ShowAsContext();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(property.FindPropertyRelative("matrix").arraySize > 0)
            return 80 + property.FindPropertyRelative("matrix").GetArrayElementAtIndex(0).FindPropertyRelative("tiles").arraySize * 12;
        else
            return 20 * 12;
    }
}
