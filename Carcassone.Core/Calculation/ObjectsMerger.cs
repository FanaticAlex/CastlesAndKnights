using Carcassone.Core.Board;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public abstract class MergableObject : BaseGameObject
    {
        public List<TileBorder> OpenBorders { get; } = new List<TileBorder>();

        public void AddPart(ObjectPart part)
        {
            Parts.Add(part);

            var newBorders = part.GetBorders();
            var union1 = OpenBorders.Except(newBorders, new BorderComparer());
            var union2 = newBorders.Except(OpenBorders, new BorderComparer());
            var uniqueToEither = union1.Union(union2).ToList();

            OpenBorders.Clear();
            OpenBorders.AddRange(uniqueToEither);
        }
    }

    /// <summary>
    /// mechanics for merging object like cities or roads
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectsMerger<T> where T : MergableObject
    {
        public void ProcessPart(List<T> Objects, ObjectPart part)
        {
            var objectsToMerge = GetObjectsToConnectWithPart(part, Objects);
            var merged = Merge(part, objectsToMerge);
            objectsToMerge.ForEach(c => Objects.Remove(c));
            Objects.Add(merged);
        }

        protected List<T> GetObjectsToConnectWithPart(ObjectPart part, List<T> objects)
        {
            var objectsToMerge = new List<T>();
            foreach (var gameObject in objects)
            {
                if (CanConnect(gameObject, part))
                    objectsToMerge.Add(gameObject);
            }

            return objectsToMerge;
        }

        public bool CanConnect(T obj, ObjectPart part)
        {
            var commonBoders = obj.OpenBorders.Intersect(part.GetBorders(), new BorderComparer());
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

