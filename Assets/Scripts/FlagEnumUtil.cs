using System;
using UnityEngine;

namespace Util
{
    public static class FlagEnumUtility
    {
        // 제네릭 함수: 플래그 Enum 타입 T에서 랜덤 값을 선택
        public static T GetRandomFlag<T>(T flags) where T : Enum
        {
            // 선택된 플래그만 필터링
            T[] selectedOptions = GetSelectedFlags(flags);

            if (selectedOptions.Length == 0)
            {
                Debug.LogWarning("No flags selected.");
                return default; // 기본값 반환 (None)
            }

            // 랜덤으로 하나 선택
            int randomIndex = UnityEngine.Random.Range(0, selectedOptions.Length);
            return selectedOptions[randomIndex];
        }

        // 제네릭 함수: 선택된 플래그 추출
        private static T[] GetSelectedFlags<T>(T flags) where T : Enum
        {
            var selectedList = new System.Collections.Generic.List<T>();
            foreach (T flag in Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt64(flags) != 0 && (Convert.ToInt64(flags) & Convert.ToInt64(flag)) != 0)
                {
                    selectedList.Add(flag);
                }
            }

            return selectedList.ToArray();
        }
    }
}