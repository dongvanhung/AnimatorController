﻿using UnityEditor;
using UnityEngine;

namespace AnimatorControllerEx {


    [CustomPropertyDrawer(typeof (AnimatorParam))]
    public class AnimatorParamDrawer : GameComponentPropertyDrawer {

        #region CONSTANTS

        // Hight of a single property.
        private const int PropHeight = 16;

        // Margin between properties.
        private const int PropMargin = 4;

        // Space between rows.
        private const int RowSpace = 8;

        // Number of rows.
        private const int Rows = 3;

        #endregion
 
        public override float GetPropertyHeight(
            SerializedProperty property,
            GUIContent label) {
            return base.GetPropertyHeight(property, label)
                   * Rows // Each row is 16 px high.
                   + (Rows - 1) * RowSpace;
        }

        public override void OnGUI(
            Rect pos,
            SerializedProperty prop,
            GUIContent label) {

            var sourceType =
                prop.FindPropertyRelative("_sourceType");
            var messageType =
                prop.FindPropertyRelative("_messageType");
            var param =
                prop.FindPropertyRelative("_param");
            var sourceCo =
                prop.FindPropertyRelative("_sourceCo");
            var sourcePropIndex =
                prop.FindPropertyRelative("_sourcePropIndex");
            var sourcePropertyName =
                prop.FindPropertyRelative("_sourcePropertyName");
            var trigger =
                prop.FindPropertyRelative("_trigger");

            DrawSourceTypeDropdown(pos, sourceType);

            // Handle selected source type.
            switch (sourceType.enumValueIndex) {
                case (int) AnimatorController.SourceTypes.Component:
                    // todo crate fields instead of passing multiple params.
                    DrawInspectorForComponentSourceType(
                        pos,
                        param,
                        trigger,
                        sourceCo,
                        sourcePropIndex,
                        sourcePropertyName);
                    break;
                case (int) AnimatorController.SourceTypes.Message:
                    DrawInspectorForMessageSourceType(
                        pos,
                        trigger,
                        messageType,
                        param);
                    break;
            }
        }

        private static void DrawInspectorForMessageSourceType(
            Rect pos,
            SerializedProperty trigger,
            SerializedProperty messageType,
            SerializedProperty param) {

            // Message type source is always a trigger.
            trigger.boolValue = true;

            DrawMessageTypeDropdown(pos, messageType);

            // Draw 'param' field.
            EditorGUIUtility.labelWidth = 50;

            // todo replace with DrawParamField. Add no param.
            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + 40,
                    pos.width * 0.5f,
                    16),
                param,
                new GUIContent(
                    "Param",
                    "Animator parameter name to update."));

            // Draw 'trigger' field.
            EditorGUIUtility.labelWidth = 50;
            // todo replace with DrawTriggerField. Add no param.
            EditorGUI.PropertyField(
                new Rect(
                    pos.x + pos.width * 0.5f + 3,
                    pos.y + 40,
                    pos.width * 0.5f,
                    16),
                trigger,
                new GUIContent(
                    "Trigger",
                    "If the animator param. is a trigger. For message " +
                    "source type it's read-only."));
        }

        private static void DrawInspectorForComponentSourceType(
            Rect pos,
            SerializedProperty param,
            SerializedProperty trigger,
            SerializedProperty sourceCo,
            SerializedProperty sourcePropIndex,
            SerializedProperty sourcePropertyName) {

            EditorGUIUtility.labelWidth = 50;

            DrawParamField(pos, param);

            EditorGUIUtility.labelWidth = 50;

            DrawTriggerField(pos, trigger);

            EditorGUIUtility.labelWidth = 50;

            DrawSourceComponentField(pos, sourceCo);

            EditorGUIUtility.labelWidth = 0;

            HandleFindComponentProperties(
                pos,
                sourceCo,
                sourcePropIndex,
                sourcePropertyName);
        }

        private static void HandleFindComponentProperties(
            Rect pos,
            SerializedProperty sourceCo,
            SerializedProperty sourcePropIndex,
            SerializedProperty sourcePropertyName) {

            // Find component properties in a selected component and display
            // as dropdown.
            if (!sourceCo.objectReferenceValue) return;

            // Get all properties from source component.
            var _sourceProperties =
                sourceCo.objectReferenceValue.GetType().GetProperties();
            // Initialize array.
            var sourcePropNames = new string[_sourceProperties.Length];
            // Fill array with property names.
            for (var i = 0; i < _sourceProperties.Length; i++) {
                sourcePropNames[i] = _sourceProperties[i].Name;
            }
            EditorGUIUtility.labelWidth = 80;
            // Display dropdown component property list.
            DrawSourcePropertyField(pos, sourcePropIndex, sourcePropNames);

            // Save selected property name.
            sourcePropertyName.stringValue =
                sourcePropNames[sourcePropIndex.intValue];
        }

        private static void DrawSourcePropertyField(
            Rect pos,
            SerializedProperty sourcePropIndex,
            string[] sourcePropNames) {

            sourcePropIndex.intValue = EditorGUI.Popup(
                new Rect(
                    pos.x,
                    pos.y + (3 * 20),
                    // rows * (row height + empty space)
                    pos.width,
                    16),
                "Source Prop.",
                sourcePropIndex.intValue,
                sourcePropNames);
        }

        private static void DrawMessageTypeDropdown(
            Rect pos,
            SerializedProperty messageType) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + 20,
                    pos.width,
                    16),
                messageType,
                new GUIContent(
                    "Message Type",
                    "Type of the message"
                    ));
        }

        private static void DrawSourceComponentField(
            Rect pos,
            SerializedProperty sourceCo) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + 2 * (PropHeight + PropMargin),
                    pos.width,
                    16),
                sourceCo,
                new GUIContent(
                    "Source",
                    "Component which property is used to update " +
                    "selected animator parameter."));
        }

        private static void DrawTriggerField(Rect pos, SerializedProperty trigger) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x + pos.width * 0.5f + 3,
                    pos.y + 1 * (PropHeight + PropMargin),
                    pos.width * 0.5f,
                    16),
                trigger,
                new GUIContent(
                    "Trigger",
                    "If the animator param. is a trigger."));
        }

        private static void DrawParamField(Rect pos, SerializedProperty param) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + 1 * (PropHeight + PropMargin),
                    pos.width * 0.5f,
                    16),
                param,
                new GUIContent(
                    "Param",
                    "Animator parameter name to update."));
        }

        private static void DrawSourceTypeDropdown(
            Rect pos,
            SerializedProperty sourceType) {

            EditorGUI.PropertyField(
                new Rect(
                    pos.x,
                    pos.y + 0 * (PropHeight + PropMargin),
                    pos.width,
                    PropHeight),
                sourceType,
                new GUIContent(
                    "Source Type",
                    "Source based on which the Animator parameter will be updated"));
        }

    }

}
