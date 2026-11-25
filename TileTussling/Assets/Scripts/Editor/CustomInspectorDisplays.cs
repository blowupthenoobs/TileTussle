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

        float cellWidth = 50;
        float cellHeight = 15;
        float spacing = 1f;

        float startX = position.x;
        float currentY = position.y + 20f;

        // Rect newPosition = position;
        // newPosition.y += 18f;

        for(int i = 0; i < rows.arraySize; i++)
        {
            SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("tiles");
            float currentX = startX;

            // Debug.Log(row.arraySize);
            for(int j = 0; j < row.arraySize; j++)
            {
                Rect cellRect = new Rect(currentX, currentY, cellWidth, cellHeight);
                EditorGUI.PropertyField(cellRect, row.GetArrayElementAtIndex(j), GUIContent.none);

                currentX += cellWidth + spacing;
                
            }
            currentY += cellHeight + spacing;

            // Rect addButton = new Rect(currentX + 20, currentY - (cellHeight + spacing), 20f, 20f);
            // if(GUI.Button(addButton, "+"))
            // {
            //     row.InsertArrayElementAtIndex(row.arraySize);
            // }
            // addButton.x += 20f;
            // if(GUI.Button(addButton, "-"))
            // {
            //     if(row.arraySize > 0)
            //         row.DeleteArrayElementAtIndex(row.arraySize - 1);
            // }
        }

        EditorGUI.EndProperty();
        Rect rowButtonRect = new Rect(position.x, currentY, 25f, 20f);
        if(GUI.Button(rowButtonRect, "+"))
        {
            rows.InsertArrayElementAtIndex(rows.arraySize);
        }
        rowButtonRect.x += 25f;
        if(GUI.Button(rowButtonRect, "-"))
        {
            if(rows.arraySize > 0)
                rows.DeleteArrayElementAtIndex(rows.arraySize - 1);
        }

        float gridHeight = rows.arraySize * (cellHeight + spacing);
        if(rows.arraySize > 0)
        {
            Rect columnButtonRect = new Rect(startX + (rows.GetArrayElementAtIndex(0).FindPropertyRelative("tiles").arraySize * (cellWidth + spacing)), position.y + 20f, 25f, gridHeight - spacing);
            if(GUI.Button(columnButtonRect, "+"))
            {
                for(int i = 0; i < rows.arraySize; i++)
                {
                    SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("tiles");
                    row.InsertArrayElementAtIndex(row.arraySize);
                    // row.GetArrayElementAtIndex(row.arraySize - 1)
                }
            }
            columnButtonRect.x += 25;
            if(GUI.Button(columnButtonRect, "-"))
            {
                for(int i = 0; i < rows.arraySize; i++)
                {
                    SerializedProperty row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("tiles");

                    if(row.arraySize > 0)
                        row.DeleteArrayElementAtIndex(row.arraySize - 1);
                    // row.GetArrayElementAtIndex(row.arraySize - 1)
                }
            }
        }
        
        Color GetTileColor(TileType type)
        {
            switch(type)
            {
                case TileType.empty:
                    return new Color(64, 64, 64, 255);
                case TileType.grass:
                    return Color.green;
                case TileType.sand:
                    return Color.yellow;
                default:
                    return Color.magenta;
            }
        }

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 80 + property.FindPropertyRelative("matrix").arraySize * 12;
    }
}
