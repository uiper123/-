using UnityEngine;
using Vuforia;

public class ElementSpawner : MonoBehaviour
{
    public ChemicalElement chemicalElement; // Ссылка на ваш ScriptableObject
    private GameObject spawnedElement;

    void Start()
    {
        // Получаем компонент ImageTarget и подписываемся на события
        var imageTarget = GetComponent<ImageTargetBehaviour>();
        if (imageTarget)
        {
            imageTarget.OnTargetStatusChanged += OnTargetStatusChanged;
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
        if (chemicalElement && chemicalElement.elementPrefab && spawnedElement == null)
        {
            // Спавним префаб как дочерний объект ImageTarget
            spawnedElement = Instantiate(chemicalElement.elementPrefab, transform);
            spawnedElement.transform.localPosition = Vector3.zero; // Устанавливаем позицию
        }
    }

    private void DespawnElement()
    {
        if (spawnedElement != null)
        {
            Destroy(spawnedElement);
            spawnedElement = null;
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
}
