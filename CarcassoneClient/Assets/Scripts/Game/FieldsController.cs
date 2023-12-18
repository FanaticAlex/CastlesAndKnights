using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    internal class FieldsController
    {
        public Dictionary<string, GameObject> _fieldsToGameObject = new();

        public List<Field> FieldsCache { get; set; } = new List<Field>();

        private readonly GameObject _fieldPrefab;
        private readonly GameObject _desk;

        public FieldsController()
        {
            _fieldPrefab = (GameObject)Resources.Load("Additional/FieldPrefab", typeof(GameObject));
            _desk = new GameObject("desk");
            CreateFieldsIfNotExistView();
        }

        /// <summary>
        /// Возвращает поле на которое указывает курсор.
        /// </summary>
        /// <returns></returns>
        public string GetSelectedFieldId()
        {
            string selectedField = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // если мы навели мышкой на поле
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0F))
            {
                foreach (var item in _fieldsToGameObject)
                {
                    var fieldObject = item.Value;
                    var field = item.Key;
                    var hitedField = hit.collider.gameObject;
                    if (hitedField == fieldObject)
                    {
                        selectedField = field;
                    }
                }
            }

            return selectedField;
        }

        /// <summary>
        /// Пересчитать доступные поля
        /// </summary>
        /// <param name="card"></param>
        public void ShowAvailableFields(Card card)
        {
            if (card == null)
            {
                foreach (var field in _fieldsToGameObject.Keys)
                    SetFieldNotAvailable(field);

                return;
            }

            var availableFields = GameManager.Instance.RoomService.GetAvailableFields(card?.Id);
            var notAvailableFields = GameManager.Instance.RoomService.GetNotAvailableFields();
            var fields = _fieldsToGameObject.Keys;
            foreach (var field in fields)
            {
                if (availableFields.Select(f => f.Id).Contains(field))
                {
                    SetFieldAvailable(field);
                }
                else if (notAvailableFields.Select(f => f.Id).Contains(field))
                {
                    SetFieldNotAvailable(field);
                }
                else
                {
                    SetFieldInvisible(field);
                }
            }
        }

        public void CreateFieldsIfNotExistView()
        {
            FieldsCache = GameManager.Instance.RoomService.GetFields();
            foreach (var field in FieldsCache)
            {
                if (!_fieldsToGameObject.Keys.Contains(field.Id))
                {
                    var fieldObject = GameObject.Instantiate(_fieldPrefab, _desk.transform);
                    fieldObject.transform.position = new Vector3(field.X, field.Y, 0);
                    fieldObject.name = field.Id;
                    _fieldsToGameObject.Add(field.Id, fieldObject);
                }
            }
        }

        private void SetFieldAvailable(string fieldId)
        {
            var image = (Sprite)Resources.Load("Additional/possible", typeof(Sprite));
            _fieldsToGameObject[fieldId].GetComponent<SpriteRenderer>().sprite = image;
        }

        private void SetFieldNotAvailable(string fieldId)
        {
            var image = (Sprite)Resources.Load("Additional/impossible", typeof(Sprite));
            _fieldsToGameObject[fieldId].GetComponent<SpriteRenderer>().sprite = image;
        }

        private void SetFieldInvisible(string fieldId)
        {
            _fieldsToGameObject[fieldId].GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
