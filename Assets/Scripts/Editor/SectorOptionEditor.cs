using UnityEngine;
using UnityEditor;

#if A
[CustomPropertyDrawer(typeof(SectorOption))]
public class SectorOptionPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // �� �ʵ��� SerializedProperty ��������
        SerializedProperty spawnOption = property.FindPropertyRelative("SpawnOption");
        SerializedProperty maxMeteorCount = property.FindPropertyRelative("MaxMeteorCount");
        SerializedProperty meteorType = property.FindPropertyRelative("MeteorType");
        SerializedProperty maxCollectableCount = property.FindPropertyRelative("MaxCollectableCount");
        SerializedProperty collectableType = property.FindPropertyRelative("CollectableType");
        SerializedProperty maxEnemyCount = property.FindPropertyRelative("MaxEnemyCount");
        SerializedProperty enemyType = property.FindPropertyRelative("EnemyType");

        // �ʵ� ���� �� ���� ����
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float sectionSpacing = 10; // ���� �� ����
        Rect fieldRect = new Rect(position.x, position.y, position.width, lineHeight);

        // SpawnOption �ʵ�
        spawnOption.intValue = (int)(MapObjectType)EditorGUI.EnumFlagsField(
            fieldRect,
            new GUIContent("Spawn Option"),
            (MapObjectType)spawnOption.intValue
        );
        fieldRect.y += lineHeight + sectionSpacing;

        // SpawnOption�� ���� ���������� �ʵ� ǥ��
        MapObjectType spawnOptionValue = (MapObjectType)spawnOption.intValue;

        if ((spawnOptionValue & MapObjectType.METEOR) == MapObjectType.METEOR)
        {
            // Meteor ����
            EditorGUI.LabelField(new Rect(fieldRect.x, fieldRect.y, fieldRect.width, lineHeight), "Meteor");
            fieldRect.y += lineHeight;

            EditorGUI.PropertyField(fieldRect, maxMeteorCount, new GUIContent("Max Meteor Count"));
            fieldRect.y += lineHeight;

            // EnumFlagsField�� MeteorType ǥ��
            meteorType.intValue = (int)(MeteorType)EditorGUI.EnumFlagsField(
                fieldRect,
                new GUIContent("Meteor Type"),
                (MeteorType)meteorType.intValue
            );
            fieldRect.y += lineHeight + sectionSpacing; // ���� �� ���� �߰�
        }

        if ((spawnOptionValue & MapObjectType.COLLECTABLE) == MapObjectType.COLLECTABLE)
        {
            // Collectable ����
            EditorGUI.LabelField(new Rect(fieldRect.x, fieldRect.y, fieldRect.width, lineHeight), "Collectable");
            fieldRect.y += lineHeight;

            EditorGUI.PropertyField(fieldRect, maxCollectableCount, new GUIContent("Max Collectable Count"));
            fieldRect.y += lineHeight;

            // EnumFlagsField�� CollectableType ǥ��
            collectableType.intValue = (int)(MeteorType)EditorGUI.EnumFlagsField(
                fieldRect,
                new GUIContent("Collectable Type"),
                (MeteorType)collectableType.intValue
            );
            fieldRect.y += lineHeight + sectionSpacing; // ���� �� ���� �߰�
        }

        if ((spawnOptionValue & MapObjectType.ENEMY) == MapObjectType.ENEMY)
        {
            // Enemy ����
            EditorGUI.LabelField(new Rect(fieldRect.x, fieldRect.y, fieldRect.width, lineHeight), "Enemy");
            fieldRect.y += lineHeight;

            EditorGUI.PropertyField(fieldRect, maxEnemyCount, new GUIContent("Max Enemy Count"));
            fieldRect.y += lineHeight;

            // EnumFlagsField�� EnemyType ǥ��
            enemyType.intValue = (int)(MeteorType)EditorGUI.EnumFlagsField(
                fieldRect,
                new GUIContent("Enemy Type"),
                (MeteorType)enemyType.intValue
            );
            fieldRect.y += lineHeight + sectionSpacing; // ���� �� ���� �߰�
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // �� �ʵ� ���� ���
        SerializedProperty spawnOption = property.FindPropertyRelative("SpawnOption");
        MapObjectType spawnOptionValue = (MapObjectType)spawnOption.intValue;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float sectionSpacing = 10; // ���� �� ����
        float height = lineHeight; // �⺻ ��

        if ((spawnOptionValue & MapObjectType.METEOR) == MapObjectType.METEOR)
        {
            height += lineHeight * 3; // Meteor �� �� �ʵ� 2�� (EnumFlagsField ����)
            height += sectionSpacing; // ���� �� ����
        }

        if ((spawnOptionValue & MapObjectType.COLLECTABLE) == MapObjectType.COLLECTABLE)
        {
            height += lineHeight * 3; // Collectable �� �� �ʵ� 2�� (EnumFlagsField ����)
            height += sectionSpacing; // ���� �� ����
        }

        if ((spawnOptionValue & MapObjectType.ENEMY) == MapObjectType.ENEMY)
        {
            height += lineHeight * 3; // Enemy �� �� �ʵ� 2�� (EnumFlagsField ����)
            height += sectionSpacing; // ���� �� ����
        }

        return height;
    }
}
#endif