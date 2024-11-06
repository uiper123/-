using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chemical Element", menuName = "ScriptableObjects/ChemicalElement", order = 1)]
public class ChemicalElement : ScriptableObject
{
    public string elementName; // Название элемента
    public GameObject elementPrefab; // Префаб элемента

    [System.Serializable]
    public class Interaction
    {
        public string otherElementName; // Название элемента для взаимодействия
        public ChemicalElement resultPrefab; // Префаб результата взаимодействия
    }

    public List<Interaction> interactions; // Список возможных взаимодействий
}
