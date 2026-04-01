using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public abstract class MergableObject : BaseGameObject
    {
        public List<TileBorder> GetOpenBorders(MoveState state)
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
        public void ConnectPartWithObjects(List<BaseGameObject> objects, ObjectPart connectingPart, MoveState state)
        {
            var objectsOfType = objects.OfType<T>();
            var objectsToMerge = GetObjectsToConnectWithPart(connectingPart, objectsOfType, state);
            var merged = Merge(connectingPart, objectsToMerge);
            objectsToMerge.ForEach(c => objects.Remove(c));
            objects.Add(merged);
        }

        protected List<T> GetObjectsToConnectWithPart(ObjectPart connectingPart, IEnumerable<T> objects, MoveState state)
        {
            var objectsToMerge = new List<T>();
            foreach (var gameObject in objects)
            {
                if (CanConnect(gameObject, connectingPart, state))
                    objectsToMerge.Add(gameObject);
            }

            return objectsToMerge;
        }

        public bool CanConnect(T obj, ObjectPart connectingPart, MoveState state)
        {
            var commonBoders = obj.GetOpenBorders(state).Intersect(connectingPart.GetBorders(state), new BorderComparer());
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

