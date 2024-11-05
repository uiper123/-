using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chemical Element", menuName = "ScriptableObjects/ChemicalElement", order = 1)]
public class ChemicalElement : ScriptableObject
{
    public string elementName; // Название элемента
    public GameObject elementPrefab; // Префаб элемента
    public List<string> compatibleElements; // Список совместимых элементов
    public List<string> incompatibleElements; // Список несовместимых элементов
}
