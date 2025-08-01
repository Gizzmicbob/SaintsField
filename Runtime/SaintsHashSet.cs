using System;
using System.Collections.Generic;
// using System.Runtime.Serialization;
using SaintsField.Utils;
using UnityEngine;

namespace SaintsField
{
    [Serializable]
    public class SaintsHashSet<T>: SaintsHashSetBase<T>
    {
        [Serializable]
        public class SaintsWrap : BaseWrap<T>
        {
            [SerializeField] public T value;
            public override T Value { get => value; set => this.value = value; }

#if UNITY_EDITOR
            // ReSharper disable once StaticMemberInGenericType
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public static readonly string EditorPropertyName = nameof(value);
#endif

            public SaintsWrap(T v)
            {
                value = v;
            }
        }

        [SerializeField]
        private List<SaintsWrap> _saintsList = new List<SaintsWrap>();

#if UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        private static string EditorPropertyName => nameof(_saintsList);
#endif

        #region Editor Interface

        protected override int SerializedCount() => _saintsList.Count;

        protected override void SerializedAdd(T key)
        {
            _saintsList.Add(new SaintsWrap(key));
        }

        protected override bool SerializedRemove(T key)
        {
            int index = _saintsList.FindIndex(each => EqualityComparer<T>.Default.Equals(each.value, key));
            if (index >= 0)
            {
                _saintsList.RemoveAt(index);
                return true;
            }

            return false;
        }

        protected override T SerializedGetAt(int index) => _saintsList[index].value;

        protected override void SerializedClear() => _saintsList.Clear();
        #endregion

        #region Constructor

        public SaintsHashSet()
        {
            HashSet = new HashSet<T>();
        }

#if UNITY_2021_2_OR_NEWER
        public SaintsHashSet(int capacity)
        {
            HashSet = new HashSet<T>(capacity);
        }
#endif

        public SaintsHashSet(IEqualityComparer<T> comparer)
        {
            HashSet = new HashSet<T>(comparer);
        }

        public SaintsHashSet(IEnumerable<T> collection)
        {
            HashSet = new HashSet<T>(collection);
        }

        public SaintsHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            switch (collection)
            {
                case null:
                    throw new ArgumentNullException(nameof(collection));
                case SaintsHashSet<T> saintsSet:
                    HashSet = new HashSet<T>(saintsSet.HashSet, comparer);
                    return;
                case HashSet<T> objSet:
                    HashSet = new HashSet<T>(objSet, comparer);
                    return;
                case ICollection<T> objs:
                    HashSet = new HashSet<T>(objs, comparer);
                    return;
                default:
                    HashSet = new HashSet<T>(comparer);
                    HashSet.UnionWith(collection);
                    return;
            }
        }

        // protected HashSet(SerializationInfo info, StreamingContext context) => HashSet.m_siInfo = info;
#if UNITY_2021_2_OR_NEWER
        public SaintsHashSet(int capacity, IEqualityComparer<T> comparer)
        {
            HashSet = new HashSet<T>(capacity, comparer);
        }
#endif

        #endregion
    }
}
