using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public class MultipartObjectsManager<T> where T : IMultipartObject
    {
        public List<T> Objects { get; set; } = new List<T>();

        public void ProcessPart(ObjectPart part, TileStack tilesStack)
        {
            var objectsToMerge = GetObjectsToConnectWithPart(part, Objects);
            var merged = Merge(part, objectsToMerge, tilesStack);
            objectsToMerge.ForEach(c => Objects.Remove(c));
            Objects.Add(merged);
        }

        protected List<T> GetObjectsToConnectWithPart(ObjectPart part, List<T> objects) 
        {
            var objectsToMerge = new List<T>();
            foreach (var gameObject in objects)
            {
                if (gameObject.CanConnect(part))
                    objectsToMerge.Add(gameObject);
            }

            return objectsToMerge;
        }

        protected T Merge(ObjectPart connectingPart, IEnumerable<T> objectsToMerge, TileStack tilesStack)
        {
            if (objectsToMerge == null)
                throw new Exception("Failed to process merge objects. objectsToMerge shouldn't be null");

            // object contained new part
            var mergedObject = (T)Activator.CreateInstance(typeof(T));
            mergedObject.AddPart(connectingPart, tilesStack);

            // Если часть присоединяется к нескольким обьектам то их можно мержить
            var allMergedParts = objectsToMerge.SelectMany(obj => obj.PartsIds.Select(id => tilesStack.GetPart(id)));
            foreach (var part in allMergedParts)
                mergedObject.AddPart(part, tilesStack);

            return mergedObject;
        }
    }
}

