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
    
    public float spawnHeight = 1.0f; // Высота спавна
    
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
            spawnedElement.transform.localPosition = new Vector3(0, spawnHeight, 0); // Устанавливаем позицию с учетом высоты спавна
            Debug.Log("Элемент заспавнен: " + spawnedElement.name);

            // Добавляем компонент плавного движения
            spawnedElement.AddComponent<SmoothFollow>().target = transform;
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

    private void SpawnElementTrig(ChemicalElement resultElement)
    {
        spawnedElementTrig = Instantiate(resultElement.elementPrefab, transform.position, Quaternion.identity);
        Debug.Log("Новый элемент заспавнен: " + spawnedElementTrig.name);
    }
}

// Компонент для плавного следования за целью
public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float speed = 2.0f;

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + new Vector3(0, 1.0f, 0); // Устанавливаем высоту следования
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        }
    }
}
