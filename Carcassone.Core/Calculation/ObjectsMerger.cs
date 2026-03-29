using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public abstract class MergableObject : BaseGameObject
    {
        public List<TileBorder> GetOpenBorders(GameState state)
        {
            var openBorders = new List<TileBorder>();
            foreach (var part in Parts)
            {
                var newBorders = part.GetBorders(state);
                var union1 = openBorders.Except(newBorders, new BorderComparer());
                var union2 = newBorders.Except(openBorders, new BorderComparer());
                openBorders = union1.Union(union2).ToList();
            }

            return openBorders;
        }

        public void AddPart(ObjectPart part)
        {
            Parts.Add(part);
        }
    }

    /// <summary>
    /// mechanics for merging object like cities or roads
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectsMerger<T> where T : MergableObject
    {
        public void ConnectPartWithObjects(List<T> objects, ObjectPart part, GameState state)
        {
            var objectsToMerge = GetObjectsToConnectWithPart(part, objects, state);
            var merged = Merge(part, objectsToMerge);
            objectsToMerge.ForEach(c => objects.Remove(c));
            objects.Add(merged);
        }

        protected List<T> GetObjectsToConnectWithPart(ObjectPart part, List<T> objects, GameState state)
        {
            var objectsToMerge = new List<T>();
            foreach (var gameObject in objects)
            {
                if (CanConnect(gameObject, part, state))
                    objectsToMerge.Add(gameObject);
            }

            return objectsToMerge;
        }

        public bool CanConnect(T obj, ObjectPart part, GameState state)
        {
            var commonBoders = obj.GetOpenBorders(state).Intersect(part.GetBorders(state), new BorderComparer());
            return commonBoders.Any();
        }

        protected T Merge(ObjectPart connectingPart, IEnumerable<T> objectsToMerge)
        {
            if (objectsToMerge == null)
                throw new Exception("Failed to process merge objects. objectsToMerge shouldn't be null");

            // new object that contains new part
            var mergedObject = (T)Activator.CreateInstance(typeof(T));
            mergedObject.AddPart(connectingPart);

            // if objects share a part then we should merge them into new object
            var allMergedParts = objectsToMerge.SelectMany(obj => obj.Parts);
            foreach (var part in allMergedParts)
                mergedObject.AddPart(part);

            return mergedObject;
        }
    }
}

