using System;
using System.Collections.Concurrent;

namespace ConceptualBrowser.Business.Common
{
    /// <summary>
    /// PHASE 4 OPTIMIZATION: Generic object pool to reduce memory allocations
    /// and improve performance by reusing expensive objects
    /// </summary>
    public class ObjectPool<T> where T : class
    {
        private readonly ConcurrentBag<T> _objects = new ConcurrentBag<T>();
        private readonly Func<T> _objectGenerator;
        private readonly Action<T> _resetAction;
        private readonly int _maxPoolSize;
        private int _currentCount;

        public ObjectPool(Func<T> objectGenerator, Action<T> resetAction = null, int maxPoolSize = 100)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
            _resetAction = resetAction;
            _maxPoolSize = maxPoolSize;
        }

        public T Rent()
        {
            if (_objects.TryTake(out T item))
            {
                return item;
            }

            return _objectGenerator();
        }

        public void Return(T item)
        {
            if (item == null) return;

            // Reset the object state if reset action is provided
            _resetAction?.Invoke(item);

            // Only add back to pool if under max size
            if (_currentCount < _maxPoolSize)
            {
                _objects.Add(item);
                _currentCount++;
            }
        }

        public void Clear()
        {
            while (_objects.TryTake(out _))
            {
                // Clear all objects from pool
            }
            _currentCount = 0;
        }
    }

    /// <summary>
    /// PHASE 4 OPTIMIZATION: Static factory for commonly pooled objects
    /// </summary>
    public static class ObjectPoolFactory
    {
        // Pool for EquivalentRectangle objects used heavily in concept extraction
        public static readonly ObjectPool<Entities.EquivalentRectangle> EquivalentRectanglePool =
            new ObjectPool<Entities.EquivalentRectangle>(
                () => new Entities.EquivalentRectangle(),
                rect => {
                    // Reset the rectangle to clean state
                    rect.Sentences.Clear();
                    rect.Keywords.Clear();
                    rect.KeywordsSentencesDictionary.Clear();
                    rect.TupleCount = 0;
                    rect.SentenceCount = 0;
                },
                maxPoolSize: 50
            );

        // Pool for EquivalentNode objects
        public static readonly ObjectPool<Entities.EquivalentNode> EquivalentNodePool =
            new ObjectPool<Entities.EquivalentNode>(
                () => new Entities.EquivalentNode(),
                node => {
                    node.Index = 0;
                    node.Indexes.Clear();
                },
                maxPoolSize: 200
            );
    }
}