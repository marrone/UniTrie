using System;
using System.Collections.Generic;

namespace UniTrie {

    public class TrieNode {

        public Dictionary<char, TrieNode> Letters;
        public bool IsWord;
        /**
         * The count of how many words this node prefixes
         */
        public int PrefixCount;

        public TrieNode() {
            Letters = new Dictionary<char, TrieNode>();
            IsWord = false;
            PrefixCount = 0;
        }

        public TrieNode Get(char letter) {
            letter = char.ToUpper(letter);
            if(Letters.TryGetValue(letter, out TrieNode childNode)) {
                return childNode;
            }
            return null;
        }

        public TrieNode Set(char letter) {
            letter = char.ToUpper(letter);
            var node = Get(letter);
            if(node == null) { 
                node = new TrieNode();
                Letters.Add(letter, node);
            }
            return node;
        }
    }

}
