using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Framework.GalaSports.Interfaces;

namespace Assets.Scripts.Framework.GalaSports.Common
{
    class Collection<T> : IEnumerable
    {
        private List<T> _list;
        private List<string> _nameList;
        public int Count { get { return _list!=null?_list.Count:0; } }

        public Collection()
        {
            _list=new List<T>();
            _nameList=new List<string>();
        }
        

        public void Add(string name,T item)
        {
            _list.Add(item);
            _nameList.Add(name);
        }

        public void RemoveLast()
        {
            _list.RemoveAt(_list.Count-1);
            _nameList.RemoveAt(_nameList.Count-1);
        }

        public void Remove(T item)
        {
            int index = _list.IndexOf(item);
            _list.RemoveAt(index);
            _nameList.RemoveAt(index);
        }

        public T GetItemByName(string name)
        {
            int index = _nameList.IndexOf(name);
            if (index != -1)
            {
                return _list[index];
            }
            else
            {
                throw new Exception("no this item");
            }
           
        }
       

        public T GetLastItem()
        {
            if (_list == null || _list.Count == 0)
            {
                throw new Exception("no item");
            }
            return _list[_list.Count - 1];
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

    }
}
