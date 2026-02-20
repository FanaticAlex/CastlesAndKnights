using Carcassone.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private Dictionary<Point, GameObject> _fieldsToGameObject = new();
        private readonly GameObject _fieldPrefab;
        private readonly GameObject _desk;


        public FieldsController()
        {
            _fieldPrefab = (GameObject)Resources.Load("Additional/FieldPrefab", typeof(GameObject));
            _desk = new GameObject("desk");
        }

        public Point? GetFieldByGameObject(GameObject go)
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

        public void ShowLocations(Locations locations)
        {
            // add new locations
            foreach (var location in locations.Awailable)
                if (!_fieldsToGameObject.Keys.Contains(location))
                    CreateLocation(location);

            // mark locations
            foreach (var location in _fieldsToGameObject.Keys)
            {
                if (locations.Awailable.Contains(location))
                {
                    SetAvailable(location);
                }
                else if (locations.UnAwailable.Contains(location))
                {
                    SetUnAvailable(location);
                }
                else
                {
                    Hide(location);
                }
            }
        }

        public void CreateLocation(Point location)
        {
            var fieldObject = GameObject.Instantiate(_fieldPrefab, _desk.transform);
            fieldObject.transform.position = new Vector3(location.X, location.Y, 0);
            fieldObject.name = $"{location.X}_{location.Y}";
            _fieldsToGameObject.Add(location, fieldObject);
        }

        private void SetAvailable(Point location)
        {
            var image = (Sprite)Resources.Load("Additional/possible", typeof(Sprite));
            _fieldsToGameObject[location].GetComponent<SpriteRenderer>().sprite = image;
        }

        private void SetUnAvailable(Point location)
        {
            var image = (Sprite)Resources.Load("Additional/impossible", typeof(Sprite));
            _fieldsToGameObject[location].GetComponent<SpriteRenderer>().sprite = image;
        }

        private void Hide(Point location)
        {
            _fieldsToGameObject[location].GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}
