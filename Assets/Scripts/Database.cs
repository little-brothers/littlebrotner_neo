
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct)]
public class AssetPathAttribute : Attribute {
	public AssetPathAttribute(string path) {
		this.Path = path;
	}

	public string Path { get; set; }
}

public interface IDatabaseRow {
	bool Parse(List<string> row);

	int ID { get; }
}

public class Database<T> where T:IDatabaseRow, new() {
	static Database<T> _instance = null;

    Dictionary<int, T> _items = new Dictionary<int, T>();

	public static Database<T> instance {
		get {
			if (_instance == null) {
				AssetPathAttribute pathAttr = typeof(T).GetCustomAttributes(typeof(AssetPathAttribute), false)[0] as AssetPathAttribute;
				Debug.Assert(pathAttr != null, "T in Database<T> must have [AssetPath] attribute!");

				_instance = new Database<T>(pathAttr.Path);
			}

			return _instance;
		}
	}

	public Database(string path)
	{
        TextAsset csv = Resources.Load(path) as TextAsset;
        if (csv == null) {
            Debug.Assert(false);
            return;
        }

        using (var streamReader = new StreamReader(new MemoryStream(csv.bytes)))
        {
            using (var reader = new Mono.Csv.CsvFileReader(streamReader))
            {
                List<string> row = new List<string>(); // use as buffer
                reader.ReadRow(row); // first row is header

                // keep read content
                while(reader.ReadRow(row)) {
                    if (row[0] == "")
                        break;

                    T elem = new T();
                    bool ret = elem.Parse(row);
                    Debug.Assert(ret);

                    _items.Add(elem.ID, elem);
                }
            }
        }

        Debug.Log(path + " loaded");
	}

    public T Find(int id) {
        if (_items.ContainsKey(id))
            return _items[id];

        return default(T);
    }

    public List<T> ToList() {
        return _items.Values.ToList();
    }
}
