using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;

public class ChemicalElementTrigger : MonoBehaviour
{
    [Header("Основной элемент для спавна")]
    [Tooltip("Название химического элемента")]
    public ChemicalElement chemicalElement; // Ссылка на ваш ScriptableObject
    private GameObject spawnedElement;

    [Header("Элемент, получаемый после взаимодействия")]
    [Tooltip("Название химического элемента")]
    public ChemicalElement chemicalElementTrig; // Ссылка на ScriptableObject химического элемента
    private GameObject spawnedElementTrig;

    [Header("Список объектов, которые заспавнились на видимых карточках")]
    public List<GameObject> foundObjects = new List<GameObject>(); // Список найденных объектов
    [Header("Список карточек")]
    public List<GameObject> ImageTargetOb = new List<GameObject>(); // Список объектов карточек
    
    private void Awake()
    {
        // Расширяем список foundObjects до размера ImageTargetOb, добавляя пустые объекты
        for (int i = 0; i < ImageTargetOb.Count; i++)
        {
            foundObjects.Add(null); // Добавляем null вместо new GameObject()
        }
    }

    void Start()
    {
        // Получаем компонент ImageTarget и подписываемся на события
        var imageTarget = GetComponent<ImageTargetBehaviour>();
        if (imageTarget)
        {
            imageTarget.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        else
        {
            Debug.LogWarning("ImageTargetBehaviour не найден на объекте!");
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        // Проверяем, что ImageTarget обнаружен
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            SpawnElement();
        }
        else
        {
            DespawnElement();
        }
    }

    private void SpawnElement()
    {
        if (chemicalElement == null)
        {
            Debug.LogError("Ссылка на ChemicalElement не установлена!");
            return;
        }

        if (chemicalElement.elementPrefab == null)
        {
            Debug.LogError("Prefab элемента не установлен в ChemicalElement!");
            return;
        }

        if (spawnedElement == null)
        {
            // Спавним префаб как дочерний объект ImageTarget
            spawnedElement = Instantiate(chemicalElement.elementPrefab, transform);
            spawnedElement.transform.localPosition = Vector3.zero; // Устанавливаем позицию
            Debug.Log("Элемент заспавнен: " + spawnedElement.name);
        }
    }

    private void DespawnElement()
    {
        if (spawnedElement != null)
        {
            Destroy(spawnedElement);
            spawnedElement = null;
            Debug.Log("Элемент деспавнен.");
        }
    }

    void OnDestroy()
    {
        // Убираем подписку на события при уничтожении объекта
        var imageTarget = GetComponent<ImageTargetBehaviour>();
        if (imageTarget)
        {
            imageTarget.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли другой объект карточкой
        if (ImageTargetOb.Contains(other.gameObject))
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

            // Проверяем взаимодействие элементов
            CheckAndSpawnResult();
        }
    }

    private void CheckAndSpawnResult()
    {
        foreach (GameObject foundObject in foundObjects)
        {
            if (foundObject != null)
            {
                foreach (ChemicalElement.Interaction interaction in chemicalElement.interactions)
                {
                   
                        chemicalElementTrig.elementPrefab = interaction.resultPrefab.elementPrefab;
                        SpawnElementTrig(chemicalElementTrig);
                        return;
                    
                }
            }
        }
    }

    private void SpawnElementTrig(ChemicalElement resultElement)
    {
        spawnedElementTrig = Instantiate(resultElement.elementPrefab, transform.position, Quaternion.identity);
        Debug.Log("Новый элемент заспавнен: " + spawnedElementTrig.name);
    }
}
