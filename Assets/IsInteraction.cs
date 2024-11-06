using System;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalElementTrigger : MonoBehaviour
{
    public ChemicalElement chemicalElement; // Ссылка на ScriptableObject химического элемента
    public List<GameObject> foundObjects = new List<GameObject>(); // Список найденных объектов
    public List<GameObject> ImageTargetOb = new List<GameObject>(); // Список объектов карточек

    private void Awake()
    {
        // Расширяем список foundObjects до размера ImageTargetOb, добавляя пустые объекты
        for (int i = 0; i < ImageTargetOb.Count; i++)
        {
            foundObjects.Add(null); // Добавляем null вместо new GameObject()
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проходим по всем объектам ImageTargetOb и ищем дочерние объекты
        for (int i = 0; i < ImageTargetOb.Count; i++)
        {
            string targetName = ImageTargetOb[i].name;
            GameObject parentObject = ImageTargetOb[i];

            // Получаем все дочерние объекты
            Transform[] childObjects = parentObject.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in childObjects)
            {
                // Проверяем, содержит ли имя дочернего объекта нужное имя
                if (child.name.Contains("Cube"))
                {
                    foundObjects[i] = child.gameObject; // Записываем найденный дочерний объект в список foundObjects
                    Debug.Log("Дочерний объект найден: " + child.name);
                    break;
                }
            }

            if (foundObjects[i] == null)
            {
                Debug.Log("Объект не найден: " + targetName);
            }
        }

        SpawmEleTr();
    }

    private void SpawmEleTr()
    {
        // Логика для спавна элементов при взаимодействии
        foreach (var obj in foundObjects)
        {
            if (obj != null)
            {
                Debug.Log("Обработка объекта: " + obj.name);
                // Добавьте здесь логику для спавна объектов
            }
        }
    }
}
