# UniTrie

A very basic [Trie](https://en.wikipedia.org/wiki/Trie) data structure for C#/Unity.

Supports only strings, but could easily be updated to support any data type if needed.

Allocation free lookups after initializating the trie with your dictionary.

Supports serialization/deserialization to/from text assets. An English asset is provided.

Comes with a simple editor window to allow generating your own serialized trie for other language sources.

## Basic Usage
```
using UniTrie;

TextAsset serializedTrie;

var Trie = Trie.Deserialize(serializedTrie.text);
trie.Contains("apple");
trie.ContainsPrefix("appl");

// or manually
var Trie = new Trie();
trie.Insert("foo");
trie.Insert("Bar");
trie.Contains("Foo"); // words are case-insensitive
trie.Delete("foo");
string serialized = trie.Serialize();
```

## Editor

Comes with a simple editor window to allow generating your own serialized trie for other language sources.

Open the window from *Window->TrieEditor*.

Select your source TextAsset file. Words should be newline seperated, one word per line.

Click *Generate Trie* to be prompted where to save your serialized text asset. Once saved you can later load that trie by calling, eg

```
Trie.Deserialize(mySerializedTrie.text); 
```

## Sample Dicts

English (US) dictionary is already provided under Dicts/en_us.txt (the source words), and Dicts/en_us-trie.txt (the serialized trie)
