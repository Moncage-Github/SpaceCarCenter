using UnityEngine;
using UnityEditor;

#if A
[CustomPropertyDrawer(typeof(SectorOption))]
public class SectorOptionPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 각 필드의 SerializedProperty 가져오기
        SerializedProperty spawnOption = property.FindPropertyRelative("SpawnOption");
        SerializedProperty maxMeteorCount = property.FindPropertyRelative("MaxMeteorCount");
        SerializedProperty meteorType = property.FindPropertyRelative("MeteorType");
        SerializedProperty maxCollectableCount = property.FindPropertyRelative("MaxCollectableCount");
        SerializedProperty collectableType = property.FindPropertyRelative("CollectableType");
        SerializedProperty maxEnemyCount = property.FindPropertyRelative("MaxEnemyCount");
        SerializedProperty enemyType = property.FindPropertyRelative("EnemyType");

        // 필드 높이 및 간격 설정
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float sectionSpacing = 10; // 섹션 간 공백
        Rect fieldRect = new Rect(position.x, position.y, position.width, lineHeight);

        // SpawnOption 필드
        spawnOption.intValue = (int)(MapObjectType)EditorGUI.EnumFlagsField(
            fieldRect,
            new GUIContent("Spawn Option"),
            (MapObjectType)spawnOption.intValue
        );
        fieldRect.y += lineHeight + sectionSpacing;

        // SpawnOption에 따라 조건적으로 필드 표시
        MapObjectType spawnOptionValue = (MapObjectType)spawnOption.intValue;

        if ((spawnOptionValue & MapObjectType.METEOR) == MapObjectType.METEOR)
        {
            // Meteor 섹션
            EditorGUI.LabelField(new Rect(fieldRect.x, fieldRect.y, fieldRect.width, lineHeight), "Meteor");
            fieldRect.y += lineHeight;

            EditorGUI.PropertyField(fieldRect, maxMeteorCount, new GUIContent("Max Meteor Count"));
            fieldRect.y += lineHeight;

            // EnumFlagsField로 MeteorType 표시
            meteorType.intValue = (int)(MeteorType)EditorGUI.EnumFlagsField(
                fieldRect,
                new GUIContent("Meteor Type"),
                (MeteorType)meteorType.intValue
            );
            fieldRect.y += lineHeight + sectionSpacing; // 섹션 간 간격 추가
        }

        if ((spawnOptionValue & MapObjectType.COLLECTABLE) == MapObjectType.COLLECTABLE)
        {
            // Collectable 섹션
            EditorGUI.LabelField(new Rect(fieldRect.x, fieldRect.y, fieldRect.width, lineHeight), "Collectable");
            fieldRect.y += lineHeight;

            EditorGUI.PropertyField(fieldRect, maxCollectableCount, new GUIContent("Max Collectable Count"));
            fieldRect.y += lineHeight;

            // EnumFlagsField로 CollectableType 표시
            collectableType.intValue = (int)(MeteorType)EditorGUI.EnumFlagsField(
                fieldRect,
                new GUIContent("Collectable Type"),
                (MeteorType)collectableType.intValue
            );
            fieldRect.y += lineHeight + sectionSpacing; // 섹션 간 간격 추가
        }

        if ((spawnOptionValue & MapObjectType.ENEMY) == MapObjectType.ENEMY)
        {
            // Enemy 섹션
            EditorGUI.LabelField(new Rect(fieldRect.x, fieldRect.y, fieldRect.width, lineHeight), "Enemy");
            fieldRect.y += lineHeight;

            EditorGUI.PropertyField(fieldRect, maxEnemyCount, new GUIContent("Max Enemy Count"));
            fieldRect.y += lineHeight;

            // EnumFlagsField로 EnemyType 표시
            enemyType.intValue = (int)(MeteorType)EditorGUI.EnumFlagsField(
                fieldRect,
                new GUIContent("Enemy Type"),
                (MeteorType)enemyType.intValue
            );
            fieldRect.y += lineHeight + sectionSpacing; // 섹션 간 간격 추가
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // 각 필드 높이 계산
        SerializedProperty spawnOption = property.FindPropertyRelative("SpawnOption");
        MapObjectType spawnOptionValue = (MapObjectType)spawnOption.intValue;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float sectionSpacing = 10; // 섹션 간 공백
        float height = lineHeight; // 기본 라벨

        if ((spawnOptionValue & MapObjectType.METEOR) == MapObjectType.METEOR)
        {
            height += lineHeight * 3; // Meteor 라벨 및 필드 2개 (EnumFlagsField 포함)
            height += sectionSpacing; // 섹션 간 간격
        }

        if ((spawnOptionValue & MapObjectType.COLLECTABLE) == MapObjectType.COLLECTABLE)
        {
            height += lineHeight * 3; // Collectable 라벨 및 필드 2개 (EnumFlagsField 포함)
            height += sectionSpacing; // 섹션 간 간격
        }

        if ((spawnOptionValue & MapObjectType.ENEMY) == MapObjectType.ENEMY)
        {
            height += lineHeight * 3; // Enemy 라벨 및 필드 2개 (EnumFlagsField 포함)
            height += sectionSpacing; // 섹션 간 간격
        }

        return height;
    }
}
#endif