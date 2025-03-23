using Carcassone.Core;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Отвечает за создание игровых полей из префабов
    /// и управление анимацией и эффектами этих обьектов.
    /// </summary>
    internal class FieldsController
    {
        private Dictionary<string, GameObject> _fieldsToGameObject = new();
        private readonly GameObject _fieldPrefab;
        private readonly GameObject _desk;

        private GameRoom _room;

        public FieldsController(GameRoom room)
        {
            _room = room;
            _fieldPrefab = (GameObject)Resources.Load("Additional/FieldPrefab", typeof(GameObject));
            _desk = new GameObject("desk");
        }

        public GameObject GetFieldGameObject(string fieldId)
        {
            return _fieldsToGameObject[fieldId];
        }

        public string GetFieldByGameObject(GameObject go)
        {
            if (go == null) { return null; }

            foreach (var item in _fieldsToGameObject)
            {
                var fieldObject = item.Value;
                var field = item.Key;
                if (go == fieldObject)
                    return field;
            }

            return null;
        }

        /// <summary>
        /// Пересчитать доступные поля
        /// </summary>
        /// <param name="card"></param>
        public void ShowAvailableFields(Card card)
        {
            if (card == null) // конец игры
            {
                foreach (var field in _fieldsToGameObject.Keys)
                    SetFieldNotAvailable(field);

                return;
            }

            var moveFields = _room.GetFieldsToPutCard(card?.Id);
            var fieldIds = _fieldsToGameObject.Keys;
            foreach (var fieldId in fieldIds)
            {
                var field = _room.FieldBoard.GetField(fieldId);
                if (moveFields.Contains(field))
                    SetFieldAvailable(fieldId);
                else
                    SetFieldInvisible(fieldId);
            }

            // недоступные поля
            var notAvailableFields = _room.RecalculateNotAvailableFields();
            foreach (var field in notAvailableFields)
                SetFieldNotAvailable(field.Id);
        }

        public void CreateFieldsIfNotExistView()
        {
            foreach (var field in _room.FieldBoard.Fields)
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
