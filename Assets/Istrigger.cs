using UnityEngine;
using System.Collections.Generic;

public class ImageTargetTrigger : MonoBehaviour
{
    // Хранит активные элементы внутри триггера
    public List<GameObject> activeElements = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, есть ли у объекта ChemicalElement компонент (или ScriptableObject)
        ElementData elementData = other.GetComponent<ElementData>();
        if (elementData != null && !activeElements.Contains(other.gameObject))
        {
            // Добавляем элемент в список активных
            activeElements.Add(other.gameObject);
            // Проверяем взаимодействия
            CheckInteractions(elementData);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Удаляем объект из списка активных при выходе из зоны
        if (activeElements.Contains(other.gameObject))
        {
            activeElements.Remove(other.gameObject);
        }
    }

    private void CheckInteractions(ElementData newElement)
    {
        // Проверяем взаимодействие нового элемента со всеми активными элементами
        foreach (var existingElement in activeElements)
        {
            if (existingElement == newElement.gameObject) continue;

            ElementData existingElementData = existingElement.GetComponent<ElementData>();
            if (existingElementData == null) continue;

            // Ищем совместимость в списке взаимодействий
            foreach (var interaction in newElement.chemicalElement.interactions)
            {
                if (interaction.otherElementName == existingElementData.chemicalElement.elementName)
                {
                    // Если найдено взаимодействие, создаем результат
                    SpawnResult(interaction.resultPrefab, newElement.gameObject, existingElement);
                    return;
                }
            }
        }
    }

    private void SpawnResult(GameObject resultPrefab, GameObject element1, GameObject element2)
    {
        if (resultPrefab != null)
        {
            // Создаем объект результата между двумя элементами
            Vector3 spawnPosition = (element1.transform.position + element2.transform.position) / 2;
            GameObject resultObject = Instantiate(resultPrefab, spawnPosition, Quaternion.identity, transform);
            Debug.Log($"Создан новый элемент: {resultPrefab.name}");

            // Удаляем оригинальные объекты, если они больше не нужны
            activeElements.Remove(element1);
            activeElements.Remove(element2);
            Destroy(element1);
            Destroy(element2);
        }
        else
        {
            Debug.LogWarning("Префаб результата не задан для взаимодействия");
        }
    }
}
