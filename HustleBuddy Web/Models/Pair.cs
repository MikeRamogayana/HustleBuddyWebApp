using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HustleBuddy_Web.Models
{
    public class Pair<K, V>
    {
        public K key { get; set; }
        public V value { get; set; }

        public Pair(K key, V value)
        {
            this.key = key;
            this.value = value;
        }

        public Pair()
        {
        }
    }
}
