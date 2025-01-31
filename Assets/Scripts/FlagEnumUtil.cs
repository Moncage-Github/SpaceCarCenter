using System;
using UnityEngine;

namespace Util
{
    public static class FlagEnumUtility
    {
        // ���׸� �Լ�: �÷��� Enum Ÿ�� T���� ���� ���� ����
        public static T GetRandomFlag<T>(T flags) where T : Enum
        {
            // ���õ� �÷��׸� ���͸�
            T[] selectedOptions = GetSelectedFlags(flags);

            if (selectedOptions.Length == 0)
            {
                Debug.LogWarning("No flags selected.");
                return default; // �⺻�� ��ȯ (None)
            }

            // �������� �ϳ� ����
            int randomIndex = UnityEngine.Random.Range(0, selectedOptions.Length);
            return selectedOptions[randomIndex];
        }

        // ���׸� �Լ�: ���õ� �÷��� ����
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